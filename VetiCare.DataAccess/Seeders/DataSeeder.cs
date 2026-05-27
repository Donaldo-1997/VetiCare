using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Enums;

namespace VetiCare.DataAccess.Seeders
{
    /// <summary>
    /// Seeder idempotente: solo inserta datos si las tablas están vacías.
    /// Llamar desde Program.cs únicamente en entorno Development.
    /// </summary>
    public static class DataSeeder
    {
        public static async Task SeedAsync(VetiCareDbContext context, UserManager<AppUser> userManager)
        {
            // Aplicar migraciones pendientes automáticamente
            await context.Database.MigrateAsync();

            await SeedBreedsAsync(context);
            await SeedVetsAsync(context);
            await SeedOwnersAsync(context);
            await SeedPetsAsync(context);
            await SeedMedicinesAsync(context);
            await SeedAppointmentsAsync(context);
            await SeedMedicalRecordsAsync(context);
            await SeedPrescriptionsAsync(context);
            await SeedUsersAsync(userManager);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 1. RAZAS
        // ─────────────────────────────────────────────────────────────────────
        private static async Task SeedBreedsAsync(VetiCareDbContext context)
        {
            if (await context.Breeds.AnyAsync()) return;

            var breeds = new List<Breed>
            {
                new() { Name = "Labrador Retriever", Species = "Perro",  CreatedAt = DateTime.UtcNow },
                new() { Name = "Golden Retriever",   Species = "Perro",  CreatedAt = DateTime.UtcNow },
                new() { Name = "Pastor Alemán",      Species = "Perro",  CreatedAt = DateTime.UtcNow },
                new() { Name = "Bulldog Francés",    Species = "Perro",  CreatedAt = DateTime.UtcNow },
                new() { Name = "Persa",              Species = "Gato",   CreatedAt = DateTime.UtcNow },
                new() { Name = "Siamés",             Species = "Gato",   CreatedAt = DateTime.UtcNow },
                new() { Name = "Maine Coon",         Species = "Gato",   CreatedAt = DateTime.UtcNow },
                new() { Name = "Común Europeo",      Species = "Gato",   CreatedAt = DateTime.UtcNow },
            };

            await context.Breeds.AddRangeAsync(breeds);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 2. VETERINARIOS
        // ─────────────────────────────────────────────────────────────────────
        private static async Task SeedVetsAsync(VetiCareDbContext context)
        {
            if (await context.Vets.AnyAsync()) return;

            var vets = new List<Vet>
            {
                new()
                {
                    FirstName     = "Alejandro",
                    LastName      = "Ramírez",
                    LicenseNumber = "VET-COL-001",
                    Specialty     = "Medicina General",
                    CreatedAt     = DateTime.UtcNow
                },
                new()
                {
                    FirstName     = "Valentina",
                    LastName      = "Ospina",
                    LicenseNumber = "VET-COL-002",
                    Specialty     = "Cirugía Veterinaria",
                    CreatedAt     = DateTime.UtcNow
                },
                new()
                {
                    FirstName     = "Carlos",
                    LastName      = "Herrera",
                    LicenseNumber = "VET-COL-003",
                    Specialty     = "Dermatología Veterinaria",
                    CreatedAt     = DateTime.UtcNow
                },
            };

            await context.Vets.AddRangeAsync(vets);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 3. PROPIETARIOS
        // ─────────────────────────────────────────────────────────────────────
        private static async Task SeedOwnersAsync(VetiCareDbContext context)
        {
            if (await context.Owners.AnyAsync()) return;

            var owners = new List<Owner>
            {
                new()
                {
                    FirstName = "María",
                    LastName  = "González",
                    Phone     = "3001234567",
                    Email     = "maria.gonzalez@email.com",
                    Address   = "Calle 10 # 5-20, Medellín",
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    FirstName = "Jorge",
                    LastName  = "Martínez",
                    Phone     = "3109876543",
                    Email     = "jorge.martinez@email.com",
                    Address   = "Carrera 45 # 80-15, Bogotá",
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    FirstName = "Laura",
                    LastName  = "Pérez",
                    Phone     = "3205551234",
                    Email     = "laura.perez@email.com",
                    Address   = "Avenida El Poblado # 12-30, Medellín",
                    CreatedAt = DateTime.UtcNow
                },
            };

            await context.Owners.AddRangeAsync(owners);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 4. MASCOTAS
        // ─────────────────────────────────────────────────────────────────────
        private static async Task SeedPetsAsync(VetiCareDbContext context)
        {
            if (await context.Pets.AnyAsync()) return;

            var owner1 = await context.Owners.FirstOrDefaultAsync(o => o.Email == "maria.gonzalez@email.com")
                         ?? throw new InvalidOperationException("Seeder: propietario 'maria.gonzalez' no encontrado.");
            var owner2 = await context.Owners.FirstOrDefaultAsync(o => o.Email == "jorge.martinez@email.com")
                         ?? throw new InvalidOperationException("Seeder: propietario 'jorge.martinez' no encontrado.");
            var owner3 = await context.Owners.FirstOrDefaultAsync(o => o.Email == "laura.perez@email.com")
                         ?? throw new InvalidOperationException("Seeder: propietario 'laura.perez' no encontrado.");

            var labrador   = await context.Breeds.FirstOrDefaultAsync(b => b.Name == "Labrador Retriever")
                             ?? throw new InvalidOperationException("Seeder: raza 'Labrador Retriever' no encontrada.");
            var goldenR    = await context.Breeds.FirstOrDefaultAsync(b => b.Name == "Golden Retriever")
                             ?? throw new InvalidOperationException("Seeder: raza 'Golden Retriever' no encontrada.");
            var pastorAlem = await context.Breeds.FirstOrDefaultAsync(b => b.Name == "Pastor Alemán")
                             ?? throw new InvalidOperationException("Seeder: raza 'Pastor Alemán' no encontrada.");
            var persa      = await context.Breeds.FirstOrDefaultAsync(b => b.Name == "Persa")
                             ?? throw new InvalidOperationException("Seeder: raza 'Persa' no encontrada.");
            var siames     = await context.Breeds.FirstOrDefaultAsync(b => b.Name == "Siamés")
                             ?? throw new InvalidOperationException("Seeder: raza 'Siamés' no encontrada.");

            var pets = new List<Pet>
            {
                new()
                {
                    Name      = "Max",
                    BirthDate = new DateTime(2020, 3, 15),
                    Weight    = 28.5f,
                    Gender    = PetGender.Male,
                    OwnerId   = owner1.Id,
                    BreedId   = labrador.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name      = "Luna",
                    BirthDate = new DateTime(2021, 7, 22),
                    Weight    = 6.2f,
                    Gender    = PetGender.Female,
                    OwnerId   = owner1.Id,
                    BreedId   = persa.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name      = "Rocky",
                    BirthDate = new DateTime(2019, 11, 5),
                    Weight    = 34.0f,
                    Gender    = PetGender.Male,
                    OwnerId   = owner2.Id,
                    BreedId   = pastorAlem.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name      = "Mia",
                    BirthDate = new DateTime(2022, 1, 10),
                    Weight    = 4.8f,
                    Gender    = PetGender.Female,
                    OwnerId   = owner2.Id,
                    BreedId   = siames.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name      = "Buddy",
                    BirthDate = new DateTime(2021, 5, 30),
                    Weight    = 22.0f,
                    Gender    = PetGender.Male,
                    OwnerId   = owner3.Id,
                    BreedId   = goldenR.Id,
                    CreatedAt = DateTime.UtcNow
                },
            };

            await context.Pets.AddRangeAsync(pets);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 5. MEDICAMENTOS
        // ─────────────────────────────────────────────────────────────────────
        private static async Task SeedMedicinesAsync(VetiCareDbContext context)
        {
            if (await context.Medicines.AnyAsync()) return;

            var medicines = new List<Medicine>
            {
                new()
                {
                    Name            = "Amoxicilina",
                    ActiveIngredient = "Amoxicilina trihidrato",
                    Unit            = "mg",
                    CreatedAt       = DateTime.UtcNow
                },
                new()
                {
                    Name            = "Meloxicam",
                    ActiveIngredient = "Meloxicam",
                    Unit            = "mg",
                    CreatedAt       = DateTime.UtcNow
                },
                new()
                {
                    Name            = "Metronidazol",
                    ActiveIngredient = "Metronidazol",
                    Unit            = "mg",
                    CreatedAt       = DateTime.UtcNow
                },
                new()
                {
                    Name            = "Ivermectina",
                    ActiveIngredient = "Ivermectina",
                    Unit            = "ml",
                    CreatedAt       = DateTime.UtcNow
                },
                new()
                {
                    Name            = "Prednisona",
                    ActiveIngredient = "Prednisona",
                    Unit            = "mg",
                    CreatedAt       = DateTime.UtcNow
                },
            };

            await context.Medicines.AddRangeAsync(medicines);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 6. CITAS (todos los estados representados)
        // ─────────────────────────────────────────────────────────────────────
        private static async Task SeedAppointmentsAsync(VetiCareDbContext context)
        {
            if (await context.Appointments.AnyAsync()) return;

            var max   = await context.Pets.FirstOrDefaultAsync(p => p.Name == "Max")
                        ?? throw new InvalidOperationException("Seeder: mascota 'Max' no encontrada. Asegúrate de ejecutar SeedPetsAsync primero.");
            var luna  = await context.Pets.FirstOrDefaultAsync(p => p.Name == "Luna")
                        ?? throw new InvalidOperationException("Seeder: mascota 'Luna' no encontrada.");
            var rocky = await context.Pets.FirstOrDefaultAsync(p => p.Name == "Rocky")
                        ?? throw new InvalidOperationException("Seeder: mascota 'Rocky' no encontrada.");
            var mia   = await context.Pets.FirstOrDefaultAsync(p => p.Name == "Mia")
                        ?? throw new InvalidOperationException("Seeder: mascota 'Mia' no encontrada.");
            var buddy = await context.Pets.FirstOrDefaultAsync(p => p.Name == "Buddy")
                        ?? throw new InvalidOperationException("Seeder: mascota 'Buddy' no encontrada.");

            var vet1 = await context.Vets.FirstOrDefaultAsync(v => v.LicenseNumber == "VET-COL-001")
                       ?? throw new InvalidOperationException("Seeder: veterinario 'VET-COL-001' no encontrado. Ejecuta Drop-Database y Update-Database para partir desde cero.");
            var vet2 = await context.Vets.FirstOrDefaultAsync(v => v.LicenseNumber == "VET-COL-002")
                       ?? throw new InvalidOperationException("Seeder: veterinario 'VET-COL-002' no encontrado.");
            var vet3 = await context.Vets.FirstOrDefaultAsync(v => v.LicenseNumber == "VET-COL-003")
                       ?? throw new InvalidOperationException("Seeder: veterinario 'VET-COL-003' no encontrado.");

            var appointments = new List<Appointment>
            {
                // ── Completadas (tienen MedicalRecord asociado más abajo)
                new()
                {
                    ScheduledAt = DateTime.UtcNow.AddDays(-10),
                    Status      = AppointmentStatus.Completed,
                    Reason      = "Control anual y vacunación",
                    PetId       = max.Id,
                    VetId       = vet1.Id,
                    CreatedAt   = DateTime.UtcNow.AddDays(-11)
                },
                new()
                {
                    ScheduledAt = DateTime.UtcNow.AddDays(-5),
                    Status      = AppointmentStatus.Completed,
                    Reason      = "Revisión por vómitos y pérdida de apetito",
                    PetId       = rocky.Id,
                    VetId       = vet1.Id,
                    CreatedAt   = DateTime.UtcNow.AddDays(-6)
                },

                // ── En progreso
                new()
                {
                    ScheduledAt = DateTime.UtcNow.AddHours(-1),
                    Status      = AppointmentStatus.InProgress,
                    Reason      = "Limpieza dental",
                    PetId       = luna.Id,
                    VetId       = vet2.Id,
                    CreatedAt   = DateTime.UtcNow.AddDays(-2)
                },

                // ── Agendadas (futuras, para probar POST y PATCH de status)
                new()
                {
                    ScheduledAt = DateTime.UtcNow.AddDays(3),
                    Status      = AppointmentStatus.Scheduled,
                    Reason      = "Revisión dermatológica — picazón persistente",
                    PetId       = mia.Id,
                    VetId       = vet3.Id,
                    CreatedAt   = DateTime.UtcNow
                },
                new()
                {
                    ScheduledAt = DateTime.UtcNow.AddDays(7),
                    Status      = AppointmentStatus.Scheduled,
                    Reason      = "Desparasitación trimestral",
                    PetId       = buddy.Id,
                    VetId       = vet1.Id,
                    CreatedAt   = DateTime.UtcNow
                },

                // ── Cancelada (para probar que no se puede editar)
                new()
                {
                    ScheduledAt = DateTime.UtcNow.AddDays(-1),
                    Status      = AppointmentStatus.Cancelled,
                    Reason      = "Cirugía de hernia — reprogramada por el dueño",
                    PetId       = rocky.Id,
                    VetId       = vet2.Id,
                    CreatedAt   = DateTime.UtcNow.AddDays(-3)
                },
            };

            await context.Appointments.AddRangeAsync(appointments);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 7. HISTORIALES MÉDICOS (solo para citas Completed)
        // ─────────────────────────────────────────────────────────────────────
        private static async Task SeedMedicalRecordsAsync(VetiCareDbContext context)
        {
            if (await context.MedicalRecords.AnyAsync()) return;

            var max   = await context.Pets.FirstOrDefaultAsync(p => p.Name == "Max")
                        ?? throw new InvalidOperationException("Seeder: mascota 'Max' no encontrada.");
            var rocky = await context.Pets.FirstOrDefaultAsync(p => p.Name == "Rocky")
                        ?? throw new InvalidOperationException("Seeder: mascota 'Rocky' no encontrada.");

            var apptMax   = await context.Appointments
                                .FirstOrDefaultAsync(a => a.PetId == max.Id && a.Status == AppointmentStatus.Completed)
                            ?? throw new InvalidOperationException("Seeder: cita completada de 'Max' no encontrada.");
            var apptRocky = await context.Appointments
                                .FirstOrDefaultAsync(a => a.PetId == rocky.Id && a.Status == AppointmentStatus.Completed)
                            ?? throw new InvalidOperationException("Seeder: cita completada de 'Rocky' no encontrada.");

            var records = new List<MedicalRecord>
            {
                new()
                {
                    Diagnosis    = "Paciente en buen estado general. Vacunación al día. Leve acumulación de sarro.",
                    Treatment    = "Vacuna polivalente aplicada. Se recomienda limpieza dental en próxima visita.",
                    Notes        = "Peso dentro del rango normal para la raza. Control en 12 meses.",
                    PetId        = max.Id,
                    AppointmentId = apptMax.Id,
                    CreatedAt    = apptMax.ScheduledAt
                },
                new()
                {
                    Diagnosis    = "Gastroenteritis aguda. Posible ingesta de cuerpo extraño.",
                    Treatment    = "Ayuno 12 horas, luego dieta blanda. Antibioticoterapia y antiinflamatorio.",
                    Notes        = "Se realiza radiografía abdominal — sin evidencia de obstrucción.",
                    PetId        = rocky.Id,
                    AppointmentId = apptRocky.Id,
                    CreatedAt    = apptRocky.ScheduledAt
                },
            };

            await context.MedicalRecords.AddRangeAsync(records);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 8. PRESCRIPCIONES
        // ─────────────────────────────────────────────────────────────────────
        private static async Task SeedPrescriptionsAsync(VetiCareDbContext context)
        {
            if (await context.Prescriptions.AnyAsync()) return;

            var rocky = await context.Pets.FirstOrDefaultAsync(p => p.Name == "Rocky")
                        ?? throw new InvalidOperationException("Seeder: mascota 'Rocky' no encontrada.");

            // Solo Rocky tiene prescripciones (gastroenteritis)
            var recordRocky = await context.MedicalRecords.FirstOrDefaultAsync(mr => mr.PetId == rocky.Id)
                              ?? throw new InvalidOperationException("Seeder: historial médico de 'Rocky' no encontrado.");

            var amoxicilina  = await context.Medicines.FirstOrDefaultAsync(m => m.Name == "Amoxicilina")
                               ?? throw new InvalidOperationException("Seeder: medicina 'Amoxicilina' no encontrada.");
            var meloxicam    = await context.Medicines.FirstOrDefaultAsync(m => m.Name == "Meloxicam")
                               ?? throw new InvalidOperationException("Seeder: medicina 'Meloxicam' no encontrada.");
            var metronidazol = await context.Medicines.FirstOrDefaultAsync(m => m.Name == "Metronidazol")
                               ?? throw new InvalidOperationException("Seeder: medicina 'Metronidazol' no encontrada.");

            var prescriptions = new List<Prescription>
            {
                new()
                {
                    Quantity        = 14,
                    Dosage          = "250 mg",
                    Instructions    = "1 tableta cada 12 horas durante 7 días, con alimento.",
                    MedicalRecordId = recordRocky.Id,
                    MedicineId      = amoxicilina.Id,
                    CreatedAt       = recordRocky.CreatedAt
                },
                new()
                {
                    Quantity        = 5,
                    Dosage          = "7.5 mg",
                    Instructions    = "1 tableta cada 24 horas durante 5 días. No administrar con el estómago vacío.",
                    MedicalRecordId = recordRocky.Id,
                    MedicineId      = meloxicam.Id,
                    CreatedAt       = recordRocky.CreatedAt
                },
                new()
                {
                    Quantity        = 10,
                    Dosage          = "250 mg",
                    Instructions    = "1 tableta cada 8 horas durante 5 días. Suspender si hay reacción adversa.",
                    MedicalRecordId = recordRocky.Id,
                    MedicineId      = metronidazol.Id,
                    CreatedAt       = recordRocky.CreatedAt
                },
            };

            await context.Prescriptions.AddRangeAsync(prescriptions);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────────────────────────────────
        // 9. USUARIO ADMIN (para pruebas de JWT)
        // ─────────────────────────────────────────────────────────────────────
        private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            const string adminEmail    = "admin@veticare.com";
            const string adminPassword = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) != null) return;

            var adminUser = new AppUser
            {
                UserName  = adminEmail,
                Email     = adminEmail,
                FirstName = "Admin",
                LastName  = "VetiCare",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al crear el usuario admin: {errors}");
            }
        }
    }
}
