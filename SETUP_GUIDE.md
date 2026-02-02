# MOM (Minutes of Meeting) Application - Complete Setup Guide

## ? Project Overview
A comprehensive ASP.NET Core web application for managing Minutes of Meetings with full CRUD operations for all entities. Uses NiceAdmin Bootstrap template for a professional UI.

---

## ?? Database Schema (7 Tables)

### 1. **MOM_MeetingType**
- Stores different types of meetings
- Fields: MeetingTypeID, MeetingTypeName, Remarks, Created, Modified

### 2. **MOM_Department**  
- Manages company departments
- Fields: DepartmentID, DepartmentName, Created, Modified

### 3. **MOM_MeetingVenue**
- Lists available meeting locations
- Fields: MeetingVenueID, MeetingVenueName, Created, Modified

### 4. **MOM_Staff**
- Employee/staff member information
- Fields: StaffID, DepartmentID (FK), StaffName, MobileNo, EmailAddress, Remarks, Created, Modified

### 5. **MOM_Meetings**
- Main meeting records
- Fields: MeetingID, MeetingDate, MeetingVenueID (FK), MeetingTypeID (FK), DepartmentID (FK), MeetingDescription, DocumentPath, Created, Modified, IsCancelled, CancellationDateTime, CancellationReason

### 6. **MOM_MeetingMember**
- Meeting attendance tracking
- Fields: MeetingMemberID, MeetingID (FK), StaffID (FK), IsPresent, Remarks, Created, Modified

---

## ?? Controllers Created (6 Total)

### 1. **HomeController**
- Route: `/Home`
- Actions: Index (Dashboard)

### 2. **DepartmentController**
- Route: `/Department`
- Actions: Index, Create, Edit, Delete, Details
- Static Sample Data: 5 departments

### 3. **MeetingTypeController**
- Route: `/MeetingType`
- Actions: Index, Create, Edit, Delete, Details
- Static Sample Data: 4 meeting types

### 4. **MeetingVenueController**
- Route: `/MeetingVenue`
- Actions: Index, Create, Edit, Delete, Details
- Static Sample Data: 4 venues

### 5. **StaffController**
- Route: `/Staff`
- Actions: Index, Create, Edit, Delete, Details
- Static Sample Data: 4 staff members

### 6. **MeetingsController**
- Route: `/Meetings`
- Actions: Index, Create, Edit, Delete, Details
- Static Sample Data: 4 meetings

### 7. **MeetingMemberController**
- Route: `/MeetingMember`
- Actions: Index, Create, Edit, Delete, Details
- Static Sample Data: 4 meeting member records

---

## ?? Views Created (28 Total)

### Department Views (4)
- `Views/Department/Index.cshtml` - List all departments
- `Views/Department/Create.cshtml` - Add new department
- `Views/Department/Edit.cshtml` - Edit department
- `Views/Department/Delete.cshtml` - Confirm delete

### MeetingType Views (4)
- `Views/MeetingType/Index.cshtml` - List all meeting types
- `Views/MeetingType/Create.cshtml` - Add new meeting type
- `Views/MeetingType/Edit.cshtml` - Edit meeting type
- `Views/MeetingType/Delete.cshtml` - Confirm delete

### MeetingVenue Views (4)
- `Views/MeetingVenue/Index.cshtml` - List all venues
- `Views/MeetingVenue/Create.cshtml` - Add new venue
- `Views/MeetingVenue/Edit.cshtml` - Edit venue
- `Views/MeetingVenue/Delete.cshtml` - Confirm delete

### Staff Views (4)
- `Views/Staff/Index.cshtml` - List all staff
- `Views/Staff/Create.cshtml` - Add new staff member
- `Views/Staff/Edit.cshtml` - Edit staff member
- `Views/Staff/Delete.cshtml` - Confirm delete

### Meetings Views (4)
- `Views/Meetings/Index.cshtml` - List all meetings
- `Views/Meetings/Create.cshtml` - Schedule new meeting
- `Views/Meetings/Edit.cshtml` - Edit meeting
- `Views/Meetings/Delete.cshtml` - Confirm delete

### MeetingMember Views (4)
- `Views/MeetingMember/Index.cshtml` - Attendance tracking
- `Views/MeetingMember/Create.cshtml` - Add member to meeting
- `Views/MeetingMember/Edit.cshtml` - Edit member attendance
- `Views/MeetingMember/Delete.cshtml` - Confirm delete

---

## ?? Models Created (6 Total)

All models are located in `Models/` directory:

1. **Dep.cs** - Department model
2. **MeetingTypeModel.cs** - MeetingType model  
3. **MeetingVenueModel.cs** - MeetingVenue model
4. **StaffModel.cs** - Staff model
5. **MeetingModel.cs** - Meeting model
6. **MeetingMember.cs** - MeetingMember model

---

## ?? Design Features

### Theme: NiceAdmin Bootstrap 5
- **Primary Color:** #4154f1 (Blue)
- **Secondary Color:** #6c757d (Gray)
- **Background:** #f6f9ff (Light Blue)

### UI Components:
- ? Professional card-based layout
- ? Responsive tables with hover effects
- ? Modal delete confirmations
- ? Form validation
- ? Breadcrumb navigation
- ? Status badges (Present/Absent, Active/Inactive)
- ? Action buttons (Edit/Delete with icons)

---

## ??? Navigation Structure

### Sidebar Menu (_Layout.cshtml)

**MASTER DATA Section:**
- Dashboard ? `/Home/Index`
- Meeting Type ? `/MeetingType/Index`
- Departments ? `/Department/Index`
- Venues ? `/MeetingVenue/Index`
- Staff Members ? `/Staff/Index`

**MEETING Section:**
- All Meetings ? `/Meetings/Index`
- Schedule Meetings ? `/Meetings/Create`
- Attendance ? `/MeetingMember/Index`

---

## ?? CRUD Operations

Each module supports:
- **Create:** Add new records via form
- **Read:** Display lists with datatable support
- **Update:** Edit existing records
- **Delete:** Soft delete with confirmation

### Example Flow: Adding a Department
1. Click "Departments" in sidebar
2. Click "+ Add Department" button
3. Fill in department name
4. Click "Create"
5. Redirects to index with new entry

---

## ?? How to Use

### Starting the Application
```bash
dotnet run
```

### Accessing Pages
- Dashboard: `https://localhost:5001/Home/Index`
- Departments: `https://localhost:5001/Department/Index`
- Meeting Types: `https://localhost:5001/MeetingType/Index`
- Venues: `https://localhost:5001/MeetingVenue/Index`
- Staff: `https://localhost:5001/Staff/Index`
- Meetings: `https://localhost:5001/Meetings/Index`
- Attendance: `https://localhost:5001/MeetingMember/Index`

---

## ?? Sample Data

Each controller initializes with sample data:

### Departments (5)
- Human Resources
- Finance
- Operations
- Marketing
- Information Technology

### Meeting Types (4)
- Regular Meeting
- Board Meeting
- Project Review
- Town Hall

### Venues (4)
- Conference Room A
- Conference Room B
- Board Room
- Main Auditorium

### Staff (4)
- John Doe (HR Manager)
- Jane Smith (Project Lead)
- Mike Johnson (Finance Manager)
- Sarah Williams (Operations Head)

### Meetings (4)
- Q4 Planning
- Budget Review
- Project Status
- Team Building

### Meeting Members (4)
- Attendance records for various meetings

---

## ? Features Implemented

### ? Dynamic Tables
- Hover effects
- Sortable columns (via datatable class)
- Action buttons (Edit/Delete)
- Responsive design

### ? Forms
- Input validation
- Error messages
- Cancel buttons
- Breadcrumb navigation

### ? Routing
- All controllers properly linked
- Navigation bar fully functional
- ASP.NET Core routing configured

### ? Bootstrap Styling
- Consistent color scheme
- Professional badges and buttons
- Proper spacing and typography
- Mobile responsive

---

## ?? Security Notes

**Current Status:** Static data only (no database)

When implementing database:
1. Add Entity Framework Core
2. Configure DbContext
3. Add validation attributes
4. Implement authorization checks
5. Add CSRF protection
6. Sanitize user inputs

---

## ?? Dependencies

- .NET 10
- ASP.NET Core MVC
- Bootstrap 5.3.3
- Bootstrap Icons
- Simple DataTables

---

## ?? Checklist for Production

- [ ] Replace static data with database
- [ ] Add user authentication
- [ ] Implement authorization rules
- [ ] Add error logging
- [ ] Configure appsettings.json
- [ ] Add unit tests
- [ ] Performance testing
- [ ] Security audit
- [ ] Documentation

---

## ?? Development Tips

### Adding a New Module
1. Create model in `Models/`
2. Create controller in `Controllers/`
3. Create views in `Views/ModuleName/`
4. Update `_Layout.cshtml` navigation
5. Add static sample data

### Customizing Styles
- Edit inline `<style>` tags in views
- Or update `wwwroot/assets/css/style.css`

### Debugging
- Use breakpoints in VS Code/Visual Studio
- Check browser console for JS errors
- Verify routing in `Program.cs`

---

## ?? Support

For issues or questions:
1. Check the routing in Program.cs
2. Verify controller names match routes
3. Clear browser cache
4. Restart application

---

**Last Updated:** 2026-01-06
**Status:** ? Complete - All CRUD operations working
**Build Status:** ? Successful
