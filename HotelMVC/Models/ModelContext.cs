using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HotelMVC.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<About> Abouts { get; set; }

    public virtual DbSet<Bankaccount> Bankaccounts { get; set; }

    public virtual DbSet<Contactus> Contactus { get; set; }

    public virtual DbSet<Home> Homes { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=xe)));User Id=C##Yara2;Password=Test321;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##YARA2")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<About>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008354");

            entity.ToTable("ABOUT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Aboutcontent)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ABOUTCONTENT");
            entity.Property(e => e.Aboutimage)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ABOUTIMAGE");
            entity.Property(e => e.Abouttitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ABOUTTITLE");
        });

        modelBuilder.Entity<Bankaccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008352");

            entity.ToTable("BANKACCOUNT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Balance)
                .HasColumnType("FLOAT")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Cardnumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cvv)
                .HasColumnType("NUMBER")
                .HasColumnName("CVV");
            entity.Property(e => e.Expirydate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRYDATE");
        });

        modelBuilder.Entity<Contactus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008358");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Contactaddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTACTADDRESS");
            entity.Property(e => e.Contactemail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CONTACTEMAIL");
            entity.Property(e => e.Contactphone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTACTPHONE");
        });

        modelBuilder.Entity<Home>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008356");

            entity.ToTable("HOME");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            
            entity.Property(e => e.Homecontent)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HOMECONTENT");
            entity.Property(e => e.Homeimage)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HOMEIMAGE");
            entity.Property(e => e.Hometitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HOMETITLE");
            entity.Property(e => e.Logo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LOGO");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Hotelid).HasName("SYS_C008340");

            entity.ToTable("HOTELS");

            entity.Property(e => e.Hotelid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Hoteladdress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HOTELADDRESS");
            entity.Property(e => e.Hoteldescription)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HOTELDESCRIPTION");
            entity.Property(e => e.Hotelemail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HOTELEMAIL");
            entity.Property(e => e.Hotelimage)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HOTELIMAGE");
            entity.Property(e => e.Hotelname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("HOTELNAME");
            entity.Property(e => e.Hotelphone)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HOTELPHONE");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Reservationid).HasName("SYS_C008372");

            entity.ToTable("RESERVATIONS");

            entity.Property(e => e.Reservationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RESERVATIONID");
            entity.Property(e => e.Checkindate)
                .HasColumnType("DATE")
                .HasColumnName("CHECKINDATE");
            entity.Property(e => e.Checkoutdate)
                .HasColumnType("DATE")
                .HasColumnName("CHECKOUTDATE");
            entity.Property(e => e.Roomid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROOMID");
            entity.Property(e => e.Totalprice)
                .HasColumnType("FLOAT")
                .HasColumnName("TOTALPRICE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Room).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Roomid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ROOMID");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_USERID2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008334");

            entity.ToTable("ROLES");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Roomid).HasName("SYS_C008369");

            entity.ToTable("ROOMS");

            entity.Property(e => e.Roomid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROOMID");
            entity.Property(e => e.Availabilitystatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("AVAILABILITYSTATUS");
            entity.Property(e => e.Hotelid)
                .HasColumnType("NUMBER")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Roomcapacity)
                .HasColumnType("NUMBER")
                .HasColumnName("ROOMCAPACITY");
            entity.Property(e => e.Roomdescription)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ROOMDESCRIPTION");
            entity.Property(e => e.Roomprice)
                .HasColumnType("FLOAT")
                .HasColumnName("ROOMPRICE");
            entity.Property(e => e.Roomimage)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ROOMIMAGE");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.Hotelid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_HOTELID");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008366");

            entity.ToTable("TESTIMONIALS");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Testimonialcontent)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIALCONTENT");
            entity.Property(e => e.Testimonialdate)
                .HasColumnType("DATE")
                .HasColumnName("TESTIMONIALDATE");
            entity.Property(e => e.Testimonialstatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIALSTATUS");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_USERID3");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008360");

            entity.ToTable("USERS");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Useremail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USEREMAIL");
            entity.Property(e => e.Userfname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERFNAME");
            entity.Property(e => e.Userlname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERLNAME");
            entity.Property(e => e.Userphone)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USERPHONE");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008362");

            entity.ToTable("USERLOGIN");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
            entity.Property(e => e.Userpassword)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERPASSWORD");

            entity.HasOne(d => d.Role).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ROLEID");

            entity.HasOne(d => d.User).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_USERID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
