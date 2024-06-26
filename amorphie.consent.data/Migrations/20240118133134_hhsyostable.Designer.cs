﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;
using amorphie.consent.data;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    [DbContext(typeof(ConsentDbContext))]
    [Migration("20240118133134_hhsyostable")]
    partial class hhsyostable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("amorphie.consent.core.Model.Consent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ClientCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ConsentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ScopeId")
                        .HasColumnType("uuid");

                    b.Property<long?>("ScopeTCKN")
                        .HasColumnType("bigint");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasComputedColumnSql("to_tsvector('english', coalesce(\"State\", '') || ' ' || coalesce(\"ConsentType\", '') || ' ' || coalesce(\"AdditionalData\", ''))", true);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StateModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<long?>("UserTCKN")
                        .HasColumnType("bigint");

                    b.Property<string>("Variant")
                        .HasColumnType("text");

                    b.Property<string>("XGroupId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SearchVector"), "GIN");

                    b.ToTable("Consents");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBAccountReference", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<string>>("AccountReferences")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<Guid>("ConsentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastValidAccessDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<List<string>>("PermissionTypes")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<DateTime?>("TransactionInquiryEndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("TransactionInquiryStartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ConsentId");

                    b.ToTable("OBAccountReferences");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBConsentIdentityInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ConsentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("IdentityData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdentityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InstitutionIdentityData")
                        .HasColumnType("text");

                    b.Property<string>("InstitutionIdentityType")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ConsentId");

                    b.ToTable("OBConsentIdentityInfos");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("HHSCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("IsUndeliverable")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastTryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ResponseCode")
                        .HasColumnType("integer");

                    b.Property<int?>("TryCount")
                        .HasColumnType("integer");

                    b.Property<string>("YOSCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OBEvents");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBEventItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EventNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OBEventId")
                        .HasColumnType("uuid");

                    b.Property<string>("SourceNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SourceType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OBEventId");

                    b.ToTable("OBEventItems");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBEventSubscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("HHSCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("XRequestId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("YOSCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OBEventSubscriptions");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBEventSubscriptionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OBEventSubscriptionId")
                        .HasColumnType("uuid");

                    b.Property<string>("SourceType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OBEventSubscriptionId");

                    b.ToTable("OBEventSubscriptionTypes");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBEventTypeSourceTypeRelation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("APIToGetData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("EventCase")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EventNotificationReporter")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EventNotificationTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsImmediateNotification")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<int?>("RetryCount")
                        .HasColumnType("integer");

                    b.Property<int?>("RetryInMinute")
                        .HasColumnType("integer");

                    b.Property<string>("RetryPolicy")
                        .HasColumnType("text");

                    b.Property<string>("SourceNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SourceType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("YOSRole")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OBEventTypeSourceTypeRelations");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBHhsInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AcikAnahtar")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ApiBilgileri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AyrikGKD")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("Durum")
                        .HasColumnType("text");

                    b.Property<string>("Kod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LogoBilgileri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Marka")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("Unv")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OBHhsInfos");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBPaymentOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ConsentDetailType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ConsentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConsentId");

                    b.ToTable("OBPaymentOrders");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBSystemEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EventNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HHSCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastTryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ResponseCode")
                        .HasColumnType("integer");

                    b.Property<string>("SourceNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SourceType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("TryCount")
                        .HasColumnType("integer");

                    b.Property<string>("XRequestId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("YOSCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OBSystemEvents");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBYosInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AcikAnahtar")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Adresler")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ApiBilgileri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<string>("Durum")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Kod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LogoBilgileri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Marka")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<List<string>>("Roller")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Unv")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OBYosInfos");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.Token", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ConsentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<int>("ExpireTime")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ModifiedByBehalfOf")
                        .HasColumnType("uuid");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasComputedColumnSql("to_tsvector('english', coalesce(\"TokenValue\", '') || ' ' || coalesce(\"TokenType\", ''))", true);

                    b.Property<string>("TokenType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TokenValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ConsentId");

                    b.HasIndex("SearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SearchVector"), "GIN");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBAccountReference", b =>
                {
                    b.HasOne("amorphie.consent.core.Model.Consent", "Consent")
                        .WithMany("OBAccountReferences")
                        .HasForeignKey("ConsentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consent");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBConsentIdentityInfo", b =>
                {
                    b.HasOne("amorphie.consent.core.Model.Consent", "Consent")
                        .WithMany("ObConsentIdentityInfos")
                        .HasForeignKey("ConsentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consent");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBEventItem", b =>
                {
                    b.HasOne("amorphie.consent.core.Model.OBEvent", "OBEvent")
                        .WithMany("OBEventItems")
                        .HasForeignKey("OBEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OBEvent");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBEventSubscriptionType", b =>
                {
                    b.HasOne("amorphie.consent.core.Model.OBEventSubscription", "OBEventSubscription")
                        .WithMany("OBEventSubscriptionTypes")
                        .HasForeignKey("OBEventSubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OBEventSubscription");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBPaymentOrder", b =>
                {
                    b.HasOne("amorphie.consent.core.Model.Consent", "Consent")
                        .WithMany("PaymentOrders")
                        .HasForeignKey("ConsentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consent");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.Token", b =>
                {
                    b.HasOne("amorphie.consent.core.Model.Consent", "Consent")
                        .WithMany("Tokens")
                        .HasForeignKey("ConsentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consent");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.Consent", b =>
                {
                    b.Navigation("OBAccountReferences");

                    b.Navigation("ObConsentIdentityInfos");

                    b.Navigation("PaymentOrders");

                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBEvent", b =>
                {
                    b.Navigation("OBEventItems");
                });

            modelBuilder.Entity("amorphie.consent.core.Model.OBEventSubscription", b =>
                {
                    b.Navigation("OBEventSubscriptionTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
