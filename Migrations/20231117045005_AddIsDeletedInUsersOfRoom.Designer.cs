﻿// <auto-generated />
using System;
using BKConnectBE.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BKConnect.Migrations
{
    [DbContext(typeof(BKConnectContext))]
    [Migration("20231117045005_AddIsDeletedInUsersOfRoom")]
    partial class AddIsDeletedInUsersOfRoom
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BKConnectBE.Model.Entities.Class", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("FacultyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Faculty", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.FriendRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ReceiverId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SendTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SenderId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<long?>("RootMessageId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SendTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SenderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TypeOfMessage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("RootMessageId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("ReceiverId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SendTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.RefreshToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Token")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Relationship", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("BlockBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("User1Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("User2Id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("User1Id");

                    b.HasIndex("User2Id");

                    b.ToTable("Relationships");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Room", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RoomType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("SchoolYearId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SchoolYearId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.RoomInvitation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ReceiverId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SendTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SenderId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("RoomId");

                    b.HasIndex("SenderId");

                    b.ToTable("RoomInvitations");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.SchoolYear", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Schemes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SchoolYears");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.UploadedFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UploadTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Avatar")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<long?>("ClassId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FacultyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastOnline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("FacultyId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.UserOfRoom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("JoinTime")
                        .HasColumnType("datetime2");

                    b.Property<long?>("ReadMessageId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ReadMessageId");

                    b.HasIndex("RoomId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersOfRoom");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Class", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.Faculty", "Faculty")
                        .WithMany("Classes")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.FriendRequest", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BKConnectBE.Model.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Message", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.Room", "Room")
                        .WithMany("Messages")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BKConnectBE.Model.Entities.Message", "RootMessage")
                        .WithMany("ReplyMessage")
                        .HasForeignKey("RootMessageId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BKConnectBE.Model.Entities.User", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Room");

                    b.Navigation("RootMessage");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.RefreshToken", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("User");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Relationship", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.User", "User1")
                        .WithMany()
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BKConnectBE.Model.Entities.User", "User2")
                        .WithMany()
                        .HasForeignKey("User2Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("User1");

                    b.Navigation("User2");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Room", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.SchoolYear", "SchoolYear")
                        .WithMany("Rooms")
                        .HasForeignKey("SchoolYearId");

                    b.Navigation("SchoolYear");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.RoomInvitation", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BKConnectBE.Model.Entities.Room", "Room")
                        .WithMany("RoomInvitations")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BKConnectBE.Model.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Receiver");

                    b.Navigation("Room");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.UploadedFile", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.Room", "Room")
                        .WithMany("UploadedFiles")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.User", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.Class", "Class")
                        .WithMany("Users")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BKConnectBE.Model.Entities.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId");

                    b.Navigation("Class");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.UserOfRoom", b =>
                {
                    b.HasOne("BKConnectBE.Model.Entities.Message", "ReadMessage")
                        .WithMany()
                        .HasForeignKey("ReadMessageId");

                    b.HasOne("BKConnectBE.Model.Entities.Room", "Room")
                        .WithMany("UsersOfRoom")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BKConnectBE.Model.Entities.User", "User")
                        .WithMany("UsersOfRoom")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ReadMessage");

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Class", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Faculty", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Message", b =>
                {
                    b.Navigation("ReplyMessage");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.Room", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("RoomInvitations");

                    b.Navigation("UploadedFiles");

                    b.Navigation("UsersOfRoom");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.SchoolYear", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("BKConnectBE.Model.Entities.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("SentMessages");

                    b.Navigation("UsersOfRoom");
                });
#pragma warning restore 612, 618
        }
    }
}