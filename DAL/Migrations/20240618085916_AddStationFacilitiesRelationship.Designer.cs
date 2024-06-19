﻿// <auto-generated />
using DAL.AppDbContextFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240618085916_AddStationFacilitiesRelationship")]
    partial class AddStationFacilitiesRelationship
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.Facilities", b =>
                {
                    b.Property<int>("Id_Stations_fr")
                        .HasColumnType("int");

                    b.Property<bool>("BikesPointPresence")
                        .HasColumnType("bit");

                    b.Property<bool>("BlueBikesPresence")
                        .HasColumnType("bit");

                    b.Property<bool>("CambioInformation")
                        .HasColumnType("bit");

                    b.Property<bool>("ConnectingBusesPresence")
                        .HasColumnType("bit");

                    b.Property<bool>("ConnectingTramPresence")
                        .HasColumnType("bit");

                    b.Property<bool>("Escalator")
                        .HasColumnType("bit");

                    b.Property<bool>("LiftOnPlatform")
                        .HasColumnType("bit");

                    b.Property<bool>("LuggageLockers")
                        .HasColumnType("bit");

                    b.Property<bool>("PMRAssistance")
                        .HasColumnType("bit");

                    b.Property<bool>("PMRToilets")
                        .HasColumnType("bit");

                    b.Property<bool>("PaidToilets")
                        .HasColumnType("bit");

                    b.Property<int>("ParkingPlacesForPMR")
                        .HasColumnType("int");

                    b.Property<bool>("TVMCount")
                        .HasColumnType("bit");

                    b.Property<bool>("Taxi")
                        .HasColumnType("bit");

                    b.Property<bool>("Wifi")
                        .HasColumnType("bit");

                    b.HasKey("Id_Stations_fr");

                    b.ToTable("facilities");
                });

            modelBuilder.Entity("DAL.Entities.Station", b =>
                {
                    b.Property<int>("Id_Stations_fr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Stations_fr"));

                    b.Property<float>("lat")
                        .HasColumnType("real");

                    b.Property<float>("lon")
                        .HasColumnType("real");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Stations_fr");

                    b.ToTable("stations_fr");
                });

            modelBuilder.Entity("DAL.Entities.Facilities", b =>
                {
                    b.HasOne("DAL.Entities.Station", "Station")
                        .WithOne("Facilities")
                        .HasForeignKey("DAL.Entities.Facilities", "Id_Stations_fr")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("DAL.Entities.Station", b =>
                {
                    b.Navigation("Facilities")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}