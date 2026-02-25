# AJAX Navigation (No Full Page Reload)

## Goal
Keep the sidebar/header static and refresh only the page content when clicking sidebar links.

## Where It Was Implemented
- `Views/Shared/_Layout.cshtml`

## Core Idea
1. Intercept sidebar link clicks with JavaScript.
2. Load the target page via `fetch` (AJAX).
3. Extract only the `#main-content` area from the response.
4. Replace current content inside `#main-content`.
5. Update the browser URL with `history.pushState`.
6. Keep sidebar and header intact.

## Key HTML Structure
```html
<main id="main" class="main">
  <div id="page-loader">...</div>
  <div id="main-content">
    @RenderBody()
  </div>
</main>
```

## Key JavaScript Logic
```javascript
const sidebarLinks = document.querySelectorAll('#sidebar-nav .nav-link');
const mainContent = document.getElementById('main-content');

async function loadPage(url) {
  const response = await fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } });
  const html = await response.text();
  const doc = new DOMParser().parseFromString(html, 'text/html');
  const newContent = doc.getElementById('main-content');
  if (newContent) {
    mainContent.innerHTML = newContent.innerHTML;
    history.pushState({ url }, '', url);
  }
}

sidebarLinks.forEach(link => {
  link.addEventListener('click', e => {
    const url = link.getAttribute('href');
    if (url && !url.startsWith('http') && !url.startsWith('#')) {
      e.preventDefault();
      loadPage(url);
    }
  });
});
```

## Extra Improvements
- Loader shown while content loads (`#page-loader`).
- Active sidebar link updated based on current URL.
- DataTables re-initialized after AJAX load.
- Back/forward navigation supported with `popstate`.

## Result
Only the content section changes. Sidebar and header remain unchanged, giving a SPA-like user experience.
