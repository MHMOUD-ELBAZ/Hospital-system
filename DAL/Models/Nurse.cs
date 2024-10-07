﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("Nurse")]
public partial class Nurse
{
    [Key]
    public int Id { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Address { get; set; }

    public Shift? Shift { get; set; }

    [StringLength(450)]
    [ForeignKey(nameof(AppUser))]
    public string? AspNetUsersId { get; set; }
    public AppUser AppUser { get; set; }

    [InverseProperty("Nurse")]
    public virtual ICollection<NursePatient> NursePatients { get; set; } = new List<NursePatient>();
}