using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateInternalMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "InternalMail");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expirydate",
                schema: "Warehouse",
                table: "WarehouseVoucherDetails",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.CreateTable(
                name: "InternalConversationParticipants",
                schema: "InternalMail",
                columns: table => new
                {
                    InternalConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastReadAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    JoinedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsMuted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalConversationParticipants_Conversation_Employee", x => new { x.InternalConversationId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_InternalConversationParticipants_Employee",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InternalConversations",
                schema: "InternalMail",
                columns: table => new
                {
                    InternalConversationId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Subject = table.Column<string>(type: "citext", nullable: false),
                    RelatedType = table.Column<int>(type: "integer", nullable: true),
                    RelatedId = table.Column<Guid>(type: "uuid", nullable: true),
                    RelatedExternalId = table.Column<string>(type: "citext", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastMessageAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedByEmployeeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalConversations_Id", x => x.InternalConversationId);
                    table.ForeignKey(
                        name: "FK_InternalConversations_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalConversations_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalConversations_DeletedByEmployee",
                        column: x => x.DeletedByEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InternalMessages",
                schema: "InternalMail",
                columns: table => new
                {
                    InternalMessageId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    InternalConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderEmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    ReplyToMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsUrgent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SentAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    EditedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EditedByEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedByEmployeeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalMessages_Id", x => x.InternalMessageId);
                    table.ForeignKey(
                        name: "FK_InternalMessages_Conversation",
                        column: x => x.InternalConversationId,
                        principalSchema: "InternalMail",
                        principalTable: "InternalConversations",
                        principalColumn: "InternalConversationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternalMessages_DeletedByEmployee",
                        column: x => x.DeletedByEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalMessages_EditedByEmployee",
                        column: x => x.EditedByEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalMessages_ReplyToMessage",
                        column: x => x.ReplyToMessageId,
                        principalSchema: "InternalMail",
                        principalTable: "InternalMessages",
                        principalColumn: "InternalMessageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalMessages_SenderEmployee",
                        column: x => x.SenderEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InternalMessageAttachments",
                schema: "InternalMail",
                columns: table => new
                {
                    InternalMessageAttachmentId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    InternalMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttachedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalMessageAttachments_Id", x => x.InternalMessageAttachmentId);
                    table.ForeignKey(
                        name: "FK_InternalMessageAttachments_Attachment",
                        column: x => x.AttachmentId,
                        principalSchema: "Attachment",
                        principalTable: "AttachmentModel",
                        principalColumn: "AttachmentModelID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalMessageAttachments_Message",
                        column: x => x.InternalMessageId,
                        principalSchema: "InternalMail",
                        principalTable: "InternalMessages",
                        principalColumn: "InternalMessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternalMessageReadStates",
                schema: "InternalMail",
                columns: table => new
                {
                    InternalMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalMessageReadStates_Message_Employee", x => new { x.InternalMessageId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_InternalMessageReadStates_Employee",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalMessageReadStates_Message",
                        column: x => x.InternalMessageId,
                        principalSchema: "InternalMail",
                        principalTable: "InternalMessages",
                        principalColumn: "InternalMessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternalConversationParticipants_Employee_Archive_Mute",
                schema: "InternalMail",
                table: "InternalConversationParticipants",
                columns: new[] { "EmployeeId", "IsArchived", "IsMuted" });

            migrationBuilder.CreateIndex(
                name: "IX_InternalConversationParticipants_Employee_LastReadAt",
                schema: "InternalMail",
                table: "InternalConversationParticipants",
                columns: new[] { "EmployeeId", "LastReadAt" });

            migrationBuilder.CreateIndex(
                name: "IX_InternalConversations_Company_Active_LastMessageAt",
                schema: "InternalMail",
                table: "InternalConversations",
                columns: new[] { "CompanyId", "IsActive", "LastMessageAt" });

            migrationBuilder.CreateIndex(
                name: "IX_InternalConversations_Company_Related",
                schema: "InternalMail",
                table: "InternalConversations",
                columns: new[] { "CompanyId", "RelatedType", "RelatedId" });

            migrationBuilder.CreateIndex(
                name: "IX_InternalConversations_CreatedBy",
                schema: "InternalMail",
                table: "InternalConversations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InternalConversations_DeletedByEmployeeId",
                schema: "InternalMail",
                table: "InternalConversations",
                column: "DeletedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalConversations_LastMessage",
                schema: "InternalMail",
                table: "InternalConversations",
                column: "LastMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessageAttachments_Attachment",
                schema: "InternalMail",
                table: "InternalMessageAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessageAttachments_Message",
                schema: "InternalMail",
                table: "InternalMessageAttachments",
                column: "InternalMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessageReadStates_Employee_IsRead",
                schema: "InternalMail",
                table: "InternalMessageReadStates",
                columns: new[] { "EmployeeId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessages_Conversation_SentAt",
                schema: "InternalMail",
                table: "InternalMessages",
                columns: new[] { "InternalConversationId", "SentAt" });

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessages_DeletedByEmployeeId",
                schema: "InternalMail",
                table: "InternalMessages",
                column: "DeletedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessages_EditedByEmployeeId",
                schema: "InternalMail",
                table: "InternalMessages",
                column: "EditedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessages_IsDeleted",
                schema: "InternalMail",
                table: "InternalMessages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessages_ReplyToMessage",
                schema: "InternalMail",
                table: "InternalMessages",
                column: "ReplyToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessages_Sender_SentAt",
                schema: "InternalMail",
                table: "InternalMessages",
                columns: new[] { "SenderEmployeeId", "SentAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_InternalConversationParticipants_Conversation",
                schema: "InternalMail",
                table: "InternalConversationParticipants",
                column: "InternalConversationId",
                principalSchema: "InternalMail",
                principalTable: "InternalConversations",
                principalColumn: "InternalConversationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InternalConversations_LastMessage",
                schema: "InternalMail",
                table: "InternalConversations",
                column: "LastMessageId",
                principalSchema: "InternalMail",
                principalTable: "InternalMessages",
                principalColumn: "InternalMessageId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalMessages_Conversation",
                schema: "InternalMail",
                table: "InternalMessages");

            migrationBuilder.DropTable(
                name: "InternalConversationParticipants",
                schema: "InternalMail");

            migrationBuilder.DropTable(
                name: "InternalMessageAttachments",
                schema: "InternalMail");

            migrationBuilder.DropTable(
                name: "InternalMessageReadStates",
                schema: "InternalMail");

            migrationBuilder.DropTable(
                name: "InternalConversations",
                schema: "InternalMail");

            migrationBuilder.DropTable(
                name: "InternalMessages",
                schema: "InternalMail");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expirydate",
                schema: "Warehouse",
                table: "WarehouseVoucherDetails",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
