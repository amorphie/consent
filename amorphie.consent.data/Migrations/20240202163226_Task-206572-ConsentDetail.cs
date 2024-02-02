using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task206572ConsentDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBAccountReferences");

            migrationBuilder.DropTable(
                name: "OBConsentIdentityInfos");

            migrationBuilder.AddColumn<string>(
                name: "StateCancelDetailCode",
                table: "Consents",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OBAccountConsentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsentId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityType = table.Column<string>(type: "text", nullable: false),
                    IdentityData = table.Column<string>(type: "text", nullable: false),
                    InstitutionIdentityType = table.Column<string>(type: "text", nullable: true),
                    InstitutionIdentityData = table.Column<string>(type: "text", nullable: true),
                    UserType = table.Column<string>(type: "text", nullable: false),
                    HhsCode = table.Column<string>(type: "text", nullable: false),
                    YosCode = table.Column<string>(type: "text", nullable: false),
                    AuthMethod = table.Column<string>(type: "text", nullable: true),
                    ForwardingAddress = table.Column<string>(type: "text", nullable: true),
                    HhsForwardingAddress = table.Column<string>(type: "text", nullable: true),
                    DiscreteGKDDefinitionType = table.Column<string>(type: "text", nullable: true),
                    DiscreteGKDDefinitionValue = table.Column<string>(type: "text", nullable: true),
                    AuthCompletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PermissionTypes = table.Column<List<string>>(type: "text[]", nullable: false),
                    LastValidAccessDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionInquiryStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TransactionInquiryEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AccountReferences = table.Column<List<string>>(type: "text[]", nullable: true),
                    OhkMessage = table.Column<string>(type: "text", nullable: true),
                    XRequestId = table.Column<string>(type: "text", nullable: false),
                    XGroupId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBAccountConsentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBAccountConsentDetails_Consents_ConsentId",
                        column: x => x.ConsentId,
                        principalTable: "Consents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OBPaymentConsentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsentId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityType = table.Column<string>(type: "text", nullable: true),
                    IdentityData = table.Column<string>(type: "text", nullable: true),
                    InstitutionIdentityType = table.Column<string>(type: "text", nullable: true),
                    InstitutionIdentityData = table.Column<string>(type: "text", nullable: true),
                    UserType = table.Column<string>(type: "text", nullable: false),
                    HhsCode = table.Column<string>(type: "text", nullable: false),
                    YosCode = table.Column<string>(type: "text", nullable: false),
                    AuthMethod = table.Column<string>(type: "text", nullable: true),
                    ForwardingAddress = table.Column<string>(type: "text", nullable: true),
                    HhsForwardingAddress = table.Column<string>(type: "text", nullable: true),
                    DiscreteGKDDefinitionType = table.Column<string>(type: "text", nullable: true),
                    DiscreteGKDDefinitionValue = table.Column<string>(type: "text", nullable: true),
                    AuthCompletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<string>(type: "text", nullable: false),
                    SenderTitle = table.Column<string>(type: "text", nullable: true),
                    SenderAccountNumber = table.Column<string>(type: "text", nullable: true),
                    SenderAccountReference = table.Column<string>(type: "text", nullable: true),
                    ReceiverTitle = table.Column<string>(type: "text", nullable: false),
                    ReceiverAccountNumber = table.Column<string>(type: "text", nullable: false),
                    KolasType = table.Column<string>(type: "text", nullable: true),
                    KolasValue = table.Column<string>(type: "text", nullable: true),
                    KolasRefNum = table.Column<long>(type: "bigint", nullable: true),
                    KolasAccountType = table.Column<string>(type: "text", nullable: true),
                    QRCodeFlowType = table.Column<string>(type: "text", nullable: true),
                    QRCodeRef = table.Column<string>(type: "text", nullable: true),
                    QRCodeProducerCode = table.Column<string>(type: "text", nullable: true),
                    PaymentSource = table.Column<string>(type: "text", nullable: false),
                    PaymentPurpose = table.Column<string>(type: "text", nullable: false),
                    ReferenceInformation = table.Column<string>(type: "text", nullable: true),
                    PaymentDescription = table.Column<string>(type: "text", nullable: true),
                    OHKMessage = table.Column<string>(type: "text", nullable: true),
                    PaymentSystem = table.Column<string>(type: "text", nullable: false),
                    ExpectedPaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WorkplaceCategoryCode = table.Column<string>(type: "text", nullable: true),
                    SubWorkplaceCategoryCode = table.Column<string>(type: "text", nullable: true),
                    GeneralWorkplaceNumber = table.Column<string>(type: "text", nullable: true),
                    XRequestId = table.Column<string>(type: "text", nullable: false),
                    XGroupId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBPaymentConsentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBPaymentConsentDetails_Consents_ConsentId",
                        column: x => x.ConsentId,
                        principalTable: "Consents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OBAccountConsentDetails_ConsentId",
                table: "OBAccountConsentDetails",
                column: "ConsentId");

            migrationBuilder.CreateIndex(
                name: "IX_OBPaymentConsentDetails_ConsentId",
                table: "OBPaymentConsentDetails",
                column: "ConsentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBAccountConsentDetails");

            migrationBuilder.DropTable(
                name: "OBPaymentConsentDetails");

            migrationBuilder.DropColumn(
                name: "StateCancelDetailCode",
                table: "Consents");

            migrationBuilder.CreateTable(
                name: "OBAccountReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountReferences = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    LastValidAccessDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    PermissionTypes = table.Column<List<string>>(type: "text[]", nullable: false),
                    TransactionInquiryEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TransactionInquiryStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBAccountReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBAccountReferences_Consents_ConsentId",
                        column: x => x.ConsentId,
                        principalTable: "Consents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OBConsentIdentityInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    IdentityData = table.Column<string>(type: "text", nullable: false),
                    IdentityType = table.Column<string>(type: "text", nullable: false),
                    InstitutionIdentityData = table.Column<string>(type: "text", nullable: true),
                    InstitutionIdentityType = table.Column<string>(type: "text", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    UserType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBConsentIdentityInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBConsentIdentityInfos_Consents_ConsentId",
                        column: x => x.ConsentId,
                        principalTable: "Consents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OBAccountReferences_ConsentId",
                table: "OBAccountReferences",
                column: "ConsentId");

            migrationBuilder.CreateIndex(
                name: "IX_OBConsentIdentityInfos_ConsentId",
                table: "OBConsentIdentityInfos",
                column: "ConsentId");
        }
    }
}
