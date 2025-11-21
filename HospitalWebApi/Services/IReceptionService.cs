/*using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalWebApi.Services
{
    public interface IReceptionService
    {
        Task<ReceptionResponseDto> RegisterOrBillAsync(ReceptionDto dto);
    }

    public class ReceptionService : IReceptionService
    {
        private readonly HospitalContext _context;

        public ReceptionService(HospitalContext context)
        {
            _context = context;
        }

        public async Task<ReceptionResponseDto> RegisterOrBillAsync(ReceptionDto dto)
        {
            // 🔹 1. Create or fetch patient
            Patient patient;
            if (dto.PatientId > 0)
            {
                patient = await _context.Patients.FindAsync(dto.PatientId)
                    ?? throw new Exception("Patient not found");
            }
            else
            {
                patient = new Patient
                {
                    PatientName = dto.PatientName,
                    Age = dto.Age,
                    Gender = dto.Gender,
                    Phone = dto.Phone,
                    Address = dto.Address
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            // 🔹 2. Create Bill Header
            var bill = new BillHeader
            {
                PatientId = patient.PatientId,
                BillDate = DateTime.Now,
                TotalAmount = 0,
                Status = "Pending"
            };
            _context.BillHeaders.Add(bill);
            await _context.SaveChangesAsync();

            // 🔹 3. Add service (optional)
            if (dto.ServiceId.HasValue)
            {
                var service = await _context.Services.FindAsync(dto.ServiceId.Value)
                    ?? throw new Exception("Service not found");

                var receipt = new ServiceReceipt
                {
                    BillHeaderId = bill.BillHeaderId,
                    ServiceId = service.ServiceId,
                    Quantity = 1,
                    Amount = service.Fee
                };
                _context.ServiceReceipts.Add(receipt);
                bill.TotalAmount += service.Fee;
            }

            // 🔹 4. Add payment (optional)
            if (dto.PaidAmount.HasValue && !string.IsNullOrEmpty(dto.PaymentMode))
            {
                var payment = new BillReceipt
                {
                    BillHeaderId = bill.BillHeaderId,
                    PaidAmount = dto.PaidAmount.Value,
                    PaymentMode = dto.PaymentMode,
                    PaymentDate = DateTime.Now
                };
                _context.BillReceipts.Add(payment);
                bill.Status = "Paid";
            }

            await _context.SaveChangesAsync();

            return new ReceptionResponseDto
            {
                BillHeaderId = bill.BillHeaderId,
                PatientId = patient.PatientId,
                PatientName = patient.PatientName,
                TotalAmount = bill.TotalAmount ?? 0,
                Status = bill.Status ?? "Pending"
            };
        }
    }
}
*/
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalWebApi.Services
{
    public interface IReceptionService
    {
        Task<ReceptionResponseDto> RegisterOrBillAsync(ReceptionDto dto);
    }

    public class ReceptionService : IReceptionService
    {
        private readonly HospitalContext _context;

        public ReceptionService(HospitalContext context)
        {
            _context = context;
        }

        public async Task<ReceptionResponseDto> RegisterOrBillAsync(ReceptionDto dto)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1) Create OR fetch patient
                Patient patient;
                if (dto.PatientId > 0)
                {
                    patient = await _context.Patients.FindAsync(dto.PatientId)
                        ?? throw new Exception("Patient not found");
                }
                else
                {
                    patient = new Patient
                    {
                        PatientName = dto.PatientName,
                        Age = dto.Age,
                        Gender = dto.Gender,
                        Phone = dto.Phone,
                        Address = dto.Address,
                        RegisteredDate = DateTime.Now
                    };

                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();

                    dto.PatientId = patient.PatientId; // return new ID
                }

                // 2) Create BillHeader
                var bill = new BillHeader
                {
                    PatientId = patient.PatientId,
                    BillDate = DateTime.Now,
                    TotalAmount = 0m,
                    Status = "Pending"
                };

                _context.BillHeaders.Add(bill);
                await _context.SaveChangesAsync(); // get BillHeaderId

              
                // 3) Add service item
                if (dto.ServiceId.HasValue)
                {
                    var service = await _context.Services.FindAsync(dto.ServiceId.Value)
                        ?? throw new Exception("Service not found");

                    var serviceReceipt = new ServiceReceipt
                    {
                        BillHeaderId = bill.BillHeaderId,
                        ServiceId = service.ServiceId,
                        Quantity = 1,
                        Amount = service.Fee   // Fee is decimal (not nullable)
                    };

                    _context.ServiceReceipts.Add(serviceReceipt);

                    // FIX: TotalAmount is decimal? so use null-coalescing
                    bill.TotalAmount = (bill.TotalAmount ?? 0m) + service.Fee;
                }


                // 4) Add payment (optional)
                if (dto.PaidAmount.HasValue && !string.IsNullOrWhiteSpace(dto.PaymentMode))
                {
                    var payment = new BillReceipt
                    {
                        BillHeaderId = bill.BillHeaderId,
                        PaidAmount = dto.PaidAmount.Value,
                        PaymentMode = dto.PaymentMode,
                        PaymentDate = DateTime.Now
                    };

                    _context.BillReceipts.Add(payment);

                    bill.Status = "Paid";
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return new ReceptionResponseDto
                {
                    BillHeaderId = bill.BillHeaderId,
                    PatientId = patient.PatientId,
                    PatientName = patient.PatientName,
                    TotalAmount = bill.TotalAmount ?? 0m,
                    Status = bill.Status ?? "Pending"
                };
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
