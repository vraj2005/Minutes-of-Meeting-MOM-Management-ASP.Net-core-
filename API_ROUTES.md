# MOM Application - Complete API & Route Reference

## ?? Application URLs

### Base URL
```
http://localhost:5001
https://localhost:5001 (HTTPS)
```

---

## ?? All Available Routes

### HOME / DASHBOARD
```
GET  /                          ? Redirect to Home/Index
GET  /Home                      ? Home/Index
GET  /Home/Index                ? Dashboard
```

---

## ?? DEPARTMENT ROUTES

### List & Display
```
GET  /Department                ? Redirect to Index
GET  /Department/Index          ? List all departments
GET  /Department/Details/{id}   ? View department details
```

### Create
```
GET  /Department/Create         ? Show create form
POST /Department/Create         ? Save new department
```

**Form Fields:**
- DepartmentName (required, string)

**Sample Data:**
1. Human Resources
2. Finance
3. Operations
4. Marketing
5. Information Technology

---

## ?? MEETING TYPE ROUTES

### List & Display
```
GET  /MeetingType               ? Redirect to Index
GET  /MeetingType/Index         ? List all meeting types
GET  /MeetingType/Details/{id}  ? View meeting type details
```

### Create
```
GET  /MeetingType/Create        ? Show create form
POST /MeetingType/Create        ? Save new meeting type
```

**Form Fields:**
- MeetingTypeName (required, string)
- Remarks (required, string)

**Sample Data:**
1. Regular Meeting
2. Board Meeting
3. Project Review
4. Town Hall

---

## ?? MEETING VENUE ROUTES

### List & Display
```
GET  /MeetingVenue              ? Redirect to Index
GET  /MeetingVenue/Index        ? List all venues
GET  /MeetingVenue/Details/{id} ? View venue details
```

### Create
```
GET  /MeetingVenue/Create       ? Show create form
POST /MeetingVenue/Create       ? Save new venue
```

**Form Fields:**
- MeetingVenueName (required, string)

**Sample Data:**
1. Conference Room A
2. Conference Room B
3. Board Room
4. Main Auditorium

---

## ?? STAFF ROUTES

### List & Display
```
GET  /Staff                     ? Redirect to Index
GET  /Staff/Index               ? List all staff members
GET  /Staff/Details/{id}        ? View staff details
```

### Create
```
GET  /Staff/Create              ? Show create form
POST /Staff/Create              ? Save new staff member
```

**Form Fields:**
- StaffName (required, string)
- DepartmentID (required, int)
- MobileNo (required, string)
- EmailAddress (required, string)
- Remarks (optional, string)

**Sample Data:**
1. John Doe (DepartmentID: 1)
2. Jane Smith (DepartmentID: 1)
3. Mike Johnson (DepartmentID: 2)
4. Sarah Williams (DepartmentID: 3)

---

## ?? MEETINGS ROUTES

### List & Display
```
GET  /Meetings                  ? Redirect to Index
GET  /Meetings/Index            ? List all meetings
GET  /Meetings/Details/{id}     ? View meeting details
```

### Create
```
GET  /Meetings/Create           ? Show create form
POST /Meetings/Create           ? Save new meeting
```

**Form Fields:**
- MeetingDate (required, datetime)
- MeetingVenueID (required, int)
- MeetingTypeID (required, int)
- DepartmentID (required, int)
- MeetingDescription (optional, string)
- DocumentPath (optional, string)

**Sample Data:**
1. Q4 Planning
2. Budget Review
3. Project Status
4. Team Building

---

## ?? MEETING MEMBER ROUTES

### List & Display
```
GET  /MeetingMember             ? Redirect to Index
GET  /MeetingMember/Index       ? List all meeting members
GET  /MeetingMember/Details/{id}? View member details
```

### Create
```
GET  /MeetingMember/Create      ? Show create form
POST /MeetingMember/Create      ? Save member attendance
```

**Form Fields:**
- MeetingID (required, int)
- StaffID (required, int)
- IsPresent (required, checkbox boolean)
- Remarks (optional, string)

**Sample Data:**
1. Meeting 1 - Staff 1 - Present
2. Meeting 1 - Staff 2 - Present
3. Meeting 2 - Staff 3 - Absent
4. Meeting 2 - Staff 4 - Present

---

## ?? EDIT ROUTES (All Modules)

### Generic Pattern
```
GET  /{Controller}/Edit/{id}    ? Show edit form
POST /{Controller}/Edit/{id}    ? Update record
```

### Examples:
```
GET  /Department/Edit/1         ? Edit Department 1
POST /Department/Edit/1         ? Update Department 1

GET  /MeetingType/Edit/2        ? Edit Meeting Type 2
POST /MeetingType/Edit/2        ? Update Meeting Type 2

GET  /Staff/Edit/3              ? Edit Staff 3
POST /Staff/Edit/3              ? Update Staff 3

GET  /Meetings/Edit/4           ? Edit Meeting 4
POST /Meetings/Edit/4           ? Update Meeting 4
```

---

## ??? DELETE ROUTES (All Modules)

### Generic Pattern
```
GET  /{Controller}/Delete/{id}  ? Show delete confirmation
POST /{Controller}/Delete/{id}  ? Delete record
```

### Examples:
```
GET  /Department/Delete/1       ? Confirm delete Department 1
POST /Department/Delete/1       ? Delete Department 1

GET  /MeetingVenue/Delete/2     ? Confirm delete Venue 2
POST /MeetingVenue/Delete/2     ? Delete Venue 2

GET  /MeetingMember/Delete/3    ? Confirm delete Member 3
POST /MeetingMember/Delete/3    ? Delete Member 3
```

---

## ?? HTTP Methods Used

| Method | Purpose | Routes |
|--------|---------|--------|
| GET | Display data/forms | /Index, /Create, /Edit, /Delete, /Details |
| POST | Submit data | /Create, /Edit, /Delete |

---

## ?? Navigation Sidebar Links

### Master Data Section
```
Dashboard      ? /Home/Index
Meeting Type   ? /MeetingType/Index
Departments    ? /Department/Index
Venues         ? /MeetingVenue/Index
Staff Members  ? /Staff/Index
```

### Meeting Section
```
All Meetings       ? /Meetings/Index
Schedule Meetings  ? /Meetings/Create
Attendance         ? /MeetingMember/Index
```

---

## ? Complete URL Examples

### Department Examples
```
1. View All:
   http://localhost:5001/Department/Index
   
2. Add New:
   http://localhost:5001/Department/Create
   
3. Edit Department 1:
   http://localhost:5001/Department/Edit/1
   
4. Delete Department 1:
   http://localhost:5001/Department/Delete/1
```

### Meeting Examples
```
1. View All:
   http://localhost:5001/Meetings/Index
   
2. Schedule New:
   http://localhost:5001/Meetings/Create
   
3. Edit Meeting 1:
   http://localhost:5001/Meetings/Edit/1
   
4. Delete Meeting 1:
   http://localhost:5001/Meetings/Delete/1
```

### Staff Examples
```
1. View All:
   http://localhost:5001/Staff/Index
   
2. Add Staff:
   http://localhost:5001/Staff/Create
   
3. Edit Staff 1:
   http://localhost:5001/Staff/Edit/1
   
4. Delete Staff 1:
   http://localhost:5001/Staff/Delete/1
```

### Attendance Examples
```
1. View Attendance:
   http://localhost:5001/MeetingMember/Index
   
2. Add Attendance:
   http://localhost:5001/MeetingMember/Create
   
3. Edit Record 1:
   http://localhost:5001/MeetingMember/Edit/1
   
4. Delete Record 1:
   http://localhost:5001/MeetingMember/Delete/1
```

---

## ?? Quick Navigation

### Start Here
```
http://localhost:5001/Home/Index
```

### Master Data Setup (In Order)
```
1. http://localhost:5001/Department/Index
2. http://localhost:5001/MeetingType/Index
3. http://localhost:5001/MeetingVenue/Index
4. http://localhost:5001/Staff/Index
```

### Meeting Management
```
1. http://localhost:5001/Meetings/Index
2. http://localhost:5001/Meetings/Create
3. http://localhost:5001/MeetingMember/Index
```

---

## ?? Route Parameter Syntax

### Single Parameter
```
/Controller/Action/{id}
/Department/Edit/1
/Staff/Delete/5
/MeetingType/Details/3
```

### Parameter Types
```
{id}     ? Integer (1, 2, 3, etc.)
{name}   ? String (department-name)
```

---

## ?? Form Submission Flow

### Create New Record Flow
```
1. Click "Add [Item]" button
   ? GET /{Controller}/Create
   
2. Fill form and click "Create"
   ? POST /{Controller}/Create
   
3. Server validates
   ? Valid: Redirect to Index
   ? Invalid: Redisplay form with errors
```

### Edit Record Flow
```
1. Click edit button (pencil icon)
   ? GET /{Controller}/Edit/{id}
   
2. Modify fields and click "Update"
   ? POST /{Controller}/Edit/{id}
   
3. Server validates and updates
   ? Valid: Redirect to Index
   ? Invalid: Redisplay form with errors
```

### Delete Record Flow
```
1. Click delete button (trash icon)
   ? GET /{Controller}/Delete/{id}
   
2. Review confirmation and click "Delete"
   ? POST /{Controller}/Delete/{id}
   
3. Server deletes record
   ? Redirect to Index
```

---

## ?? Default Route Behavior

```
/Controller                    ? Redirects to /Controller/Index
/Controller/                   ? Redirects to /Controller/Index
/                              ? Redirects to /Home/Index
/Home                          ? Redirects to /Home/Index
```

---

## ?? Important Notes

1. **ID Parameter:** All Edit/Delete routes require ID
2. **POST Methods:** Form submissions use POST
3. **Validation:** Server-side validation on create/edit
4. **Redirects:** Success redirects back to Index
5. **Case-Sensitive:** URLs are case-insensitive (ASP.NET Core default)

---

## ?? Testing Routes

### Test All CRUD for Department:
```bash
# 1. List
http://localhost:5001/Department/Index

# 2. Create Form
http://localhost:5001/Department/Create

# 3. Edit Form
http://localhost:5001/Department/Edit/1

# 4. Delete Form
http://localhost:5001/Department/Delete/1
```

### Repeat for other modules:
- /MeetingType
- /MeetingVenue
- /Staff
- /Meetings
- /MeetingMember

---

**Complete Route Map Generated:** 2026-01-06
**Total Unique Routes:** 42+
**Status:** ? All routes functional
