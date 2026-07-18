using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs.Configurations.OrderSchema
{
    public class PurchaseOrderDocumentConfiguration : IEntityTypeConfiguration<PurchaseOrderDocument>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderDocument> entity)
        {
            entity.HasKey(e => e.PurchaseOrderDocumentId)
                .HasName("PK_PurchaseOrderDocuments_PurchaseOrderDocumentId");

            entity.ToTable("PurchaseOrderDocuments", "Orders");

            entity.Property(e => e.PurchaseOrderDocumentId)
                .HasColumnName("PurchaseOrderDocumentId")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.DocumentType)
                .HasColumnName("DocumentType")
                .HasConversion<int>();

            entity.Property(e => e.DocumentCode)
                .HasColumnName("DocumentCode")
                .HasColumnType("citext");

            entity.Property(e => e.DocumentName)
                .HasColumnName("DocumentName")
                .HasColumnType("citext");

            entity.Property(e => e.Note)
                .HasColumnName("Note")
                .HasColumnType("text");

            entity.Property(e => e.IsReceived)
                .HasColumnName("IsReceived")
                .HasDefaultValue(false);

            entity.Property(e => e.ReceivedDate)
                .HasColumnName("ReceivedDate");

            entity.Property(e => e.VerifiedBy)
                .HasColumnName("VerifiedBy");

            entity.Property(e => e.VerifiedDate)
                .HasColumnName("VerifiedDate");

            entity.Property(e => e.CreatedDate)
                .HasColumnName("CreatedDate");

            entity.Property(e => e.CreatedBy)
                .HasColumnName("CreatedBy");

            entity.Property(e => e.UpdatedDate)
                .HasColumnName("UpdatedDate");

            entity.Property(e => e.UpdatedBy)
                .HasColumnName("UpdatedBy");

            entity.Property(e => e.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            entity.HasIndex(e => e.PurchaseOrderId)
                .HasDatabaseName("IX_PurchaseOrderDocuments_PurchaseOrderId");

            entity.HasIndex(e => new { e.PurchaseOrderId, e.DocumentType, e.IsActive })
                .HasDatabaseName("IX_PurchaseOrderDocuments_PO_DocumentType_IsActive");

            entity.HasIndex(e => e.AttachmentCollectionId)
                .HasDatabaseName("IX_PurchaseOrderDocuments_AttachmentCollectionId");

            entity.HasOne(e => e.PurchaseOrder)
                .WithMany(e => e.PurchaseOrderDocuments)
                .HasForeignKey(e => e.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PurchaseOrderDocuments_PurchaseOrders");

            entity.HasOne(e => e.AttachmentCollection)
                .WithMany()
                .HasForeignKey(e => e.AttachmentCollectionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PurchaseOrderDocuments_AttachmentCollection");

            entity.HasOne(e => e.CreatedByNavigation)
                .WithMany(e => e.PurchaseOrderDocumentCreatedByNavigations)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PurchaseOrderDocuments_CreatedBy");

            entity.HasOne(e => e.UpdatedByNavigation)
                .WithMany(e => e.PurchaseOrderDocumentUpdatedByNavigations)
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PurchaseOrderDocuments_UpdatedBy");

            entity.HasOne(e => e.VerifiedByNavigation)
                .WithMany(e => e.PurchaseOrderDocumentVerifiedByNavigations)
                .HasForeignKey(e => e.VerifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PurchaseOrderDocuments_VerifiedBy");
        }
    }
}
