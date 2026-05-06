using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetiCare.Domain.Entities;
public class Prescription : AuditBase
{
    public int Quantity { get; set; }
    public string Dosage { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;

    // Foreign Keys
    public int MedicalRecordId { get; set; }
    public int MedicineId { get; set; }

    // Navigation Properties
    public MedicalRecord MedicalRecord { get; set; } = null!;
    public Medicine Medicine { get; set; } = null!;
}
