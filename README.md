# MusicMingle

**A music-event discovery and booking site built with ASP.NET Core Razor Pages and Identity.**
Visitors browse events by category, sign up, and book; admins run the whole catalogue from a role-protected back office.

![.NET](https://img.shields.io/badge/.NET_9-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF_Core-512BD4?logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?logo=bootstrap&logoColor=white)

<!-- Add screenshots: events list, event details + booking, admin events -->

---

## Features

**For visitors**
- Browse events with text search and a category filter; event detail pages
- Register / log in with ASP.NET Core Identity
- Book events and see them under **My Bookings**
- Contact form

**For admins** (`[Authorize(Roles = "Admin")]`)
- Full CRUD for events and categories
- View all bookings
- Manage user accounts and roles (Admin / User / Visitor)

## Tech stack

ASP.NET Core 9 Razor Pages · ASP.NET Core Identity with roles · Entity Framework Core (code-first migrations) · SQLite · Bootstrap

## Data model

`Event` · `Category` · `Booking` · `UserEvent` · `ContactMessage` + the Identity user/role tables.

## Run it

**Requirements:** .NET 9 SDK

```bash
git clone https://github.com/shamsullahfayzi/MusicMingle.git
cd MusicMingle/MusicMingle
dotnet tool install --global dotnet-ef   # once
dotnet ef database update                # creates app.db from the migrations
dotnet run
```

The **first account you register becomes the Admin**; every later account is a regular user.

## Author

**Shamsullah Fayzi** · [GitHub](https://github.com/shamsullahfayzi)
