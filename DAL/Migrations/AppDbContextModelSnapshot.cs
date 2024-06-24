﻿// <auto-generated />
using DAL.AppDbContextFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.BusStop", b =>
                {
                    b.Property<string>("StopId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Lat")
                        .HasColumnType("float");

                    b.Property<double>("Lon")
                        .HasColumnType("float");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StopName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StopId");

                    b.ToTable("busStop");
                });

            modelBuilder.Entity("DAL.Entities.Facilities", b =>
                {
                    b.Property<int>("Id_Station")
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

                    b.Property<bool>("FreeToilets")
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

                    b.HasKey("Id_Station");

                    b.ToTable("facilities");
                });

            modelBuilder.Entity("DAL.Entities.Platforms", b =>
                {
                    b.Property<string>("Perron_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Hauteur")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id_Quai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id_Station")
                        .HasColumnType("int");

                    b.Property<string>("LongNameDutch")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LongNameFrench")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Perron_Id");

                    b.HasIndex("Id_Station");

                    b.ToTable("platforms");
                });

            modelBuilder.Entity("DAL.Entities.Station", b =>
                {
                    b.Property<int>("Id_Station")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Station"));

                    b.Property<string>("Official_Station_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("lat")
                        .HasColumnType("real");

                    b.Property<float>("lon")
                        .HasColumnType("real");

                    b.Property<string>("name_eng")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name_fr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name_nl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Station");

                    b.ToTable("stations");
                });

            modelBuilder.Entity("DAL.Entities.Facilities", b =>
                {
                    b.HasOne("DAL.Entities.Station", "Station")
                        .WithOne("Facilities")
                        .HasForeignKey("DAL.Entities.Facilities", "Id_Station")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("DAL.Entities.Platforms", b =>
                {
                    b.HasOne("DAL.Entities.Station", null)
                        .WithMany("Platforms")
                        .HasForeignKey("Id_Station")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL.Entities.Station", b =>
                {
                    b.Navigation("Facilities")
                        .IsRequired();

                    b.Navigation("Platforms");
                });
#pragma warning restore 612, 618
        }
    }
}
