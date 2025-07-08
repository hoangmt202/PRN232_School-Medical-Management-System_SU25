using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class ini2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugStorages_SchoolNurses_ManagedBy",
                table: "DrugStorages");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentReports_SchoolNurses_NurseId",
                table: "IncidentReports");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Students_StudentId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_SchoolNurses_GivenBy",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Users_UserId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolNurses_Users_UserId",
                table: "SchoolNurses");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolNurses",
                table: "SchoolNurses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalRecords",
                table: "MedicalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrugStorages",
                table: "DrugStorages");

            migrationBuilder.RenameTable(
                name: "SchoolNurses",
                newName: "school_nurses");

            migrationBuilder.RenameTable(
                name: "MedicalRecords",
                newName: "medical_records");

            migrationBuilder.RenameTable(
                name: "DrugStorages",
                newName: "drug_storage");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Students",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "Class",
                table: "Students",
                newName: "class");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Students",
                newName: "parent_id");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Students",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Students",
                newName: "date_of_birth");

            migrationBuilder.RenameIndex(
                name: "IX_Students_ParentId",
                table: "Students",
                newName: "IX_Students_parent_id");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Parents",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Parents",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Parents",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Parents",
                newName: "full_name");

            migrationBuilder.RenameIndex(
                name: "IX_Parents_UserId",
                table: "Parents",
                newName: "IX_Parents_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "school_nurses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "school_nurses",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "school_nurses",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "school_nurses",
                newName: "full_name");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolNurses_UserId",
                table: "school_nurses",
                newName: "IX_school_nurses_user_id");

            migrationBuilder.RenameColumn(
                name: "Allergies",
                table: "medical_records",
                newName: "allergies");

            migrationBuilder.RenameColumn(
                name: "TreatmentHistory",
                table: "medical_records",
                newName: "treatment_history");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "medical_records",
                newName: "student_id");

            migrationBuilder.RenameColumn(
                name: "PhysicalCondition",
                table: "medical_records",
                newName: "physical_condition");

            migrationBuilder.RenameColumn(
                name: "ChronicDiseases",
                table: "medical_records",
                newName: "chronic_diseases");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_StudentId",
                table: "medical_records",
                newName: "IX_medical_records_student_id");

            migrationBuilder.RenameColumn(
                name: "Strength",
                table: "drug_storage",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "drug_storage",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Manufacturer",
                table: "drug_storage",
                newName: "manufacturer");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "drug_storage",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StorageLocation",
                table: "drug_storage",
                newName: "storage_location");

            migrationBuilder.RenameColumn(
                name: "MedicationName",
                table: "drug_storage",
                newName: "medication_name");

            migrationBuilder.RenameColumn(
                name: "ManagedBy",
                table: "drug_storage",
                newName: "managed_by");

            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "drug_storage",
                newName: "expiration_date");

            migrationBuilder.RenameColumn(
                name: "DosageForm",
                table: "drug_storage",
                newName: "dosage_form");

            migrationBuilder.RenameColumn(
                name: "DateReceived",
                table: "drug_storage",
                newName: "date_received");

            migrationBuilder.RenameIndex(
                name: "IX_DrugStorages_ManagedBy",
                table: "drug_storage",
                newName: "IX_drug_storage_managed_by");

            migrationBuilder.AlterColumn<string>(
                name: "strength",
                table: "drug_storage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "manufacturer",
                table: "drug_storage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "storage_location",
                table: "drug_storage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "medication_name",
                table: "drug_storage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "dosage_form",
                table: "drug_storage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_school_nurses",
                table: "school_nurses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_medical_records",
                table: "medical_records",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_drug_storage",
                table: "drug_storage",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_drug_storage_school_nurses_managed_by",
                table: "drug_storage",
                column: "managed_by",
                principalTable: "school_nurses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentReports_school_nurses_NurseId",
                table: "IncidentReports",
                column: "NurseId",
                principalTable: "school_nurses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_medical_records_Students_student_id",
                table: "medical_records",
                column: "student_id",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_school_nurses_GivenBy",
                table: "Medications",
                column: "GivenBy",
                principalTable: "school_nurses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Users_user_id",
                table: "Parents",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_school_nurses_Users_user_id",
                table: "school_nurses",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Parents_parent_id",
                table: "Students",
                column: "parent_id",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_drug_storage_school_nurses_managed_by",
                table: "drug_storage");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentReports_school_nurses_NurseId",
                table: "IncidentReports");

            migrationBuilder.DropForeignKey(
                name: "FK_medical_records_Students_student_id",
                table: "medical_records");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_school_nurses_GivenBy",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Users_user_id",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_school_nurses_Users_user_id",
                table: "school_nurses");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Parents_parent_id",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_school_nurses",
                table: "school_nurses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_medical_records",
                table: "medical_records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_drug_storage",
                table: "drug_storage");

            migrationBuilder.RenameTable(
                name: "school_nurses",
                newName: "SchoolNurses");

            migrationBuilder.RenameTable(
                name: "medical_records",
                newName: "MedicalRecords");

            migrationBuilder.RenameTable(
                name: "drug_storage",
                newName: "DrugStorages");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "gender",
                table: "Students",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "class",
                table: "Students",
                newName: "Class");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "Students",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "Students",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "date_of_birth",
                table: "Students",
                newName: "DateOfBirth");

            migrationBuilder.RenameIndex(
                name: "IX_Students_parent_id",
                table: "Students",
                newName: "IX_Students_ParentId");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Parents",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Parents",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Parents",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "Parents",
                newName: "FullName");

            migrationBuilder.RenameIndex(
                name: "IX_Parents_user_id",
                table: "Parents",
                newName: "IX_Parents_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SchoolNurses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "SchoolNurses",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "SchoolNurses",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "SchoolNurses",
                newName: "FullName");

            migrationBuilder.RenameIndex(
                name: "IX_school_nurses_user_id",
                table: "SchoolNurses",
                newName: "IX_SchoolNurses_UserId");

            migrationBuilder.RenameColumn(
                name: "allergies",
                table: "MedicalRecords",
                newName: "Allergies");

            migrationBuilder.RenameColumn(
                name: "treatment_history",
                table: "MedicalRecords",
                newName: "TreatmentHistory");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "MedicalRecords",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "physical_condition",
                table: "MedicalRecords",
                newName: "PhysicalCondition");

            migrationBuilder.RenameColumn(
                name: "chronic_diseases",
                table: "MedicalRecords",
                newName: "ChronicDiseases");

            migrationBuilder.RenameIndex(
                name: "IX_medical_records_student_id",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_StudentId");

            migrationBuilder.RenameColumn(
                name: "strength",
                table: "DrugStorages",
                newName: "Strength");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "DrugStorages",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "manufacturer",
                table: "DrugStorages",
                newName: "Manufacturer");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "DrugStorages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "storage_location",
                table: "DrugStorages",
                newName: "StorageLocation");

            migrationBuilder.RenameColumn(
                name: "medication_name",
                table: "DrugStorages",
                newName: "MedicationName");

            migrationBuilder.RenameColumn(
                name: "managed_by",
                table: "DrugStorages",
                newName: "ManagedBy");

            migrationBuilder.RenameColumn(
                name: "expiration_date",
                table: "DrugStorages",
                newName: "ExpirationDate");

            migrationBuilder.RenameColumn(
                name: "dosage_form",
                table: "DrugStorages",
                newName: "DosageForm");

            migrationBuilder.RenameColumn(
                name: "date_received",
                table: "DrugStorages",
                newName: "DateReceived");

            migrationBuilder.RenameIndex(
                name: "IX_drug_storage_managed_by",
                table: "DrugStorages",
                newName: "IX_DrugStorages_ManagedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Strength",
                table: "DrugStorages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Manufacturer",
                table: "DrugStorages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "StorageLocation",
                table: "DrugStorages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MedicationName",
                table: "DrugStorages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "DosageForm",
                table: "DrugStorages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolNurses",
                table: "SchoolNurses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalRecords",
                table: "MedicalRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrugStorages",
                table: "DrugStorages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DrugStorages_SchoolNurses_ManagedBy",
                table: "DrugStorages",
                column: "ManagedBy",
                principalTable: "SchoolNurses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentReports_SchoolNurses_NurseId",
                table: "IncidentReports",
                column: "NurseId",
                principalTable: "SchoolNurses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Students_StudentId",
                table: "MedicalRecords",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_SchoolNurses_GivenBy",
                table: "Medications",
                column: "GivenBy",
                principalTable: "SchoolNurses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Users_UserId",
                table: "Parents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolNurses_Users_UserId",
                table: "SchoolNurses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Parents_ParentId",
                table: "Students",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
