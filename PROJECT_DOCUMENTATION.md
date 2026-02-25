# MOM Management System (ASP.NET Core) — Project Documentation

> Scope: This document explains what the project does, how it is structured, which technologies are used (backend + frontend), how database + stored procedures integrate with the code, and a deep walk‑through of **CRUD implementation for the Staff module** (controller + views + modal UI).
>
> Repository: `MOM_Project`

---

## 1) Project Overview

**MOM Management System** is a web application for maintaining master data and meeting records for “Minutes of Meeting (MOM)” workflows.

It supports:
- Master data maintenance:
  - Departments
  - Meeting Types
  - Meeting Venues
  - Staff Members
- Meeting scheduling:
  - Meetings
- Attendance tracking:
  - Meeting Members / Attendance

The application follows classic server-rendered ASP.NET patterns:
- Controllers return Views (Razor `.cshtml`)
- SQL Server is accessed using **ADO.NET** (`SqlConnection`, `SqlCommand`, stored procedures)

---

## 2) Tech Stack

### Backend
- **.NET 10** (Target framework)
- **C# 14**
- **ASP.NET Core MVC** (Controllers + Razor Views)
- **ADO.NET** for SQL access
  - `Microsoft.Data.SqlClient`
  - `System.Data`

### Frontend
- **Razor Views** (`.cshtml`) for server-side HTML templating
- **Bootstrap 5**
  - Modals for Create/Edit/Delete UI
  - Buttons, layout, grid
- **Simple-DataTables** (`simple-datatables`)
  - Sorting, pagination, search on tables
- A template script: `wwwroot/assets/js/main.js`

### Important Frontend Note: AJAX Navigation
The layout includes an **AJAX navigation** script that loads pages via `fetch()` and swaps `#main-content`.

This improves perceived performance, but has one key consequence:
- **Inline scripts inside views do not execute** when HTML is injected via `innerHTML`.

To fix this for modal value population (Edit/Delete), the project uses a global initializer in:
- `Views/Shared/_Layout.cshtml` → `initializePageBehaviors()`

This binds Bootstrap modal events (`show.bs.modal`) so modals always populate even after AJAX navigation.

---

## 3) Database Design (SQL Server)

Database name: `MOM_DB`

### Tables (high level)

1. `MOM_Department`
- `DepartmentID` (PK, identity)
- `DepartmentName`
- `Remarks`
- `Created`, `Modified`

2. `MOM_MeetingType`
- `MeetingTypeID` (PK)
- `MeetingTypeName`, `Remarks`, `Created`, `Modified`

3. `MOM_MeetingVenue`
- `MeetingVenueID` (PK)
- `MeetingVenueName`, `Remarks`, `Created`, `Modified`

4. `MOM_Staff`
- `StaffID` (PK)
- `DepartmentID` (FK → `MOM_Department.DepartmentID`)
- `StaffName`, `Mobile`, `Email`, `Remarks`
- `Created`, `Modified`

5. `MOM_Meetings`
- `MeetingID` (PK)
- `MeetingDate`
- `MeetingTypeID` (FK)
- `DepartmentID` (FK)
- `MeetingVenueID` (FK)
- `MeetingDescription`, `DocumentPath`
- `IsCancelled`, `CancellationDateTime`, `CancellationReason`
- `Created`, `Modified`

6. `MOM_MeetingMember`
- `MeetingMemberID` (PK)
- `MeetingID` (FK → `MOM_Meetings.MeetingID`)
- `StaffID` (FK → `MOM_Staff.StaffID`)
- `IsPresent`
- `Remarks`

---

## 4) Stored Procedure Pattern

Every module generally has the same set of Stored Procedures:
- `SelectAll`
- `SelectByPK`
- `Insert`
- `UpdateByPK`
- `DeleteByPK`

Example for Staff:
- `PR_MOM_Staff_SelectAll`
- `PR_MOM_Staff_SelectByPK`
- `PR_MOM_Staff_Insert`
- `PR_MOM_Staff_UpdateByPK`
- `PR_MOM_Staff_DeleteByPK`

This design keeps SQL logic in the DB and keeps controllers simple.

---

## 5) Application Structure

Common folders:
- `Controllers/` → request handlers (MVC controllers)
- `Models/` → POCO classes (Department, Staff, Meeting, etc.)
- `Views/` → Razor views
  - `Views/Staff/Index.cshtml` etc.
- `Views/Shared/_Layout.cshtml` → common layout + sidebar + scripts
- `wwwroot/` → static assets (css/js/vendor)

Routing:
- Typical route pattern: `/{Controller}/{Action}/{id?}`
- Example: `Staff/Index`, `Staff/DeleteConfirmed`

---

## 6) How CRUD Works (General)

### Backend (Controller)
All controllers follow a similar ADO.NET approach:
1. Read DB connection string from config:
   - `configuration.GetConnectionString("MOM_DB")`
2. Use `SqlConnection`.
3. Use `SqlCommand` with `CommandType.StoredProcedure`.
4. Add parameters using `command.Parameters.AddWithValue()`.
5. Execute:
   - `ExecuteReader()` for `Select` results
   - `ExecuteNonQuery()` for `Insert/Update/Delete`
6. Try/catch guards failures and adds `ModelState` errors.

### Frontend (Views)
Most screens use:
- A table with `.datatable` class for Simple-DataTables.
- Bootstrap modals for Create/Edit/Delete.
- For Edit/Delete, row buttons carry `data-*` attributes such as:
  - `data-id`, `data-name`, `data-remarks`
- The modal inputs are filled when the modal opens.

Because the app uses AJAX navigation, modal filling is handled globally in `_Layout.cshtml`.

---

## 7) Deep Dive: Staff Module (Full Explanation)

This section explains Staff module end-to-end (Index + Create + Edit + Delete).

### 7.1 Model: `Models/StaffModel.cs`
The Staff model represents the `MOM_Staff` table plus derived display data.
Typical fields used in the app:
- `StaffID`
- `DepartmentID`
- `StaffName`
- `MobileNo` (maps to DB column `Mobile`)
- `EmailAddress` (maps to DB column `Email`)
- `Remarks`
- `Created`, `Modified`
- `DepartmentName` (from join in SelectAll)

### 7.2 Controller: `Controllers/StaffController.cs`

#### Connection string
The controller stores `_connectionString` in the constructor.
This is used for every action.

#### (A) Index (Read)
`Index()` loads:
1. Departments list (for dropdowns) using `LoadDepartments()`.
2. Staff list using `PR_MOM_Staff_SelectAll`.

Key points:
- `PR_MOM_Staff_SelectAll` returns joined info including `DepartmentName`.
- The code maps DB rows → `Staff` objects.
- `ViewBag.Departments` is set so the view can populate the Department dropdown.

#### (B) Create (Insert)
`Create(Staff staff)` is a POST action.

Validation:
- Requires `StaffName` and valid `DepartmentID`.

Database call:
- Calls `dbo.PR_MOM_Staff_Insert`
- Parameters:
  - `@DepartmentID`
  - `@StaffName`
  - `@Mobile`
  - `@Email`
  - `@Remarks`

Null handling:
- Optional fields are passed as `DBNull.Value` when null.

After success:
- Redirect to `Index` so the list is refreshed.

#### (C) Edit (Update)
`Edit(Staff staff)` is a POST action.

Validation:
- Requires `StaffID`, `DepartmentID`, `StaffName`.

Database call:
- Calls `dbo.PR_MOM_Staff_UpdateByPK`
- Parameters:
  - `@StaffID`
  - `@DepartmentID`
  - `@StaffName`
  - `@Mobile`
  - `@Email`
  - `@Remarks`

After success:
- Redirect to `Index`.

#### (D) Delete (Delete)
There are two steps:

1) `Delete(int id)` (GET)
- Fetches staff record by ID using `GetStaffById(id)`.
- Returns a confirmation view with the correct `Model`.

2) `DeleteConfirmed(int staffId)` (POST)
- Called by the delete form.
- Executes `dbo.PR_MOM_Staff_DeleteByPK`.
- If failure happens, it reloads staff info and returns Delete view with an error.

Important binding detail:
- The delete form posts an input named `staffId` so it binds to `DeleteConfirmed(int staffId)`.

### 7.3 Staff Views

#### `Views/Staff/Index.cshtml`
- Shows staff list in a datatable.
- Contains modals for Create/Edit/Delete.

**Delete Modal population**
- Delete button contains:
  - `data-id="@staff.StaffID"`
  - `data-name="@staff.StaffName"`

Because of AJAX navigation, the layout’s `initializePageBehaviors()` binds:
- `show.bs.modal` for the Staff delete modal (so hidden input + name are populated reliably)

#### `Views/Staff/Delete.cshtml`
- Dedicated confirmation page (non-modal).
- Posts to `DeleteConfirmed`.
- Uses:
  - `<input type="hidden" name="staffId" value="@Model.StaffID" />`

---

## 8) Meetings & Attendance (High-level)

### Meetings
- Controller: `Controllers/MeetingsController.cs`
- View: `Views/Meetings/Index.cshtml`

Uses:
- `PR_MOM_Meetings_SelectAll` for listing
- `PR_MOM_Meetings_SelectByPK` for full meeting details
- Insert/Update/Delete stored procs for CRUD

### Attendance (MeetingMember)
- Controller: `Controllers/MeetingMemberController.cs`
- View: `Views/MeetingMember/Index.cshtml`

Uses:
- `PR_MOM_MeetingMember_SelectAll` etc.

Dropdowns use:
- Meetings list (Meeting IDs)
- Staff list (Staff IDs)

---

## 9) Common Issues & Fixes

### Issue: Modal values blank on first navigation, work after refresh
**Root cause:** AJAX navigation injects HTML but does not execute view scripts.

**Fix:** Use global modal initializers in `_Layout.cshtml` (`initializePageBehaviors`) and bind to Bootstrap `show.bs.modal`.

---

## 10) How to Run (Checklist)

1. Ensure SQL Server is running.
2. Create DB `MOM_DB` and tables/SPs.
3. Configure connection string `MOM_DB` in `appsettings.json` (not shown here if not committed).
4. Run the project.

---

## 11) Interview Ready Summary

If asked “how does CRUD work here?” (example Staff):
- UI uses Bootstrap modals/forms.
- Forms post to controller actions.
- Controller uses ADO.NET with stored procedures.
- SelectAll renders data into view model list.
- Insert/Update/Delete call their SPs with parameters.
- Redirect to Index after mutation.
- AJAX navigation requires re-init of modal scripts globally.

If asked “why stored procedures?”
- Centralized DB logic, easy governance, consistent CRUD contract, reduces inline SQL.

If asked “how is frontend built?”
- Razor Views + Bootstrap + Simple-DataTables, with layout-driven AJAX navigation.

---

### Appendix: Important Files
- `Controllers/StaffController.cs`
- `Views/Staff/Index.cshtml`
- `Views/Staff/Delete.cshtml`
- `Views/Shared/_Layout.cshtml`
- `Models/StaffModel.cs`
- DB SPs: `PR_MOM_Staff_*`
