﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TelegramBot.DbAccess;

namespace KosarRB_Bot.Migrations
{
    [DbContext(typeof(TelegramBotContext))]
    partial class TelegramBotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("TelegramBot.Models.BotMessage", b =>
                {
                    b.Property<int>("fldid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("fldMes")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("fldMobileNumberOrId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("fldOK")
                        .HasColumnType("INTEGER");

                    b.Property<string>("fldResponse")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("fldSendTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("fldTime")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("flddate")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("fldid");

                    b.ToTable("tblBotMessage");
                });

            modelBuilder.Entity("TelegramBot.Models.ContactInfo", b =>
                {
                    b.Property<long>("fldChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("fldChatState")
                        .HasColumnType("INTEGER");

                    b.Property<int>("fldChatType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("fldMobileNumberOrId")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("fldChatId");

                    b.ToTable("tblContactInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
