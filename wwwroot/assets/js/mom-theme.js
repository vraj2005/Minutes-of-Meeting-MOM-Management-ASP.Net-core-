(function () {
	const storageKey = 'mom_theme';

	function getPreferredTheme() {
		const stored = localStorage.getItem(storageKey);
		if (stored === 'light' || stored === 'dark') return stored;
		return (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) ? 'dark' : 'light';
	}

	function applyTheme(theme) {
		document.documentElement.dataset.theme = theme;
		localStorage.setItem(storageKey, theme);

		const icon = document.getElementById('mom-theme-icon');
		const label = document.getElementById('mom-theme-label');
		if (icon) icon.className = theme === 'dark' ? 'bi bi-moon-stars' : 'bi bi-sun';
		if (label) label.textContent = theme === 'dark' ? 'Dark' : 'Light';
	}

	function toggleTheme() {
		const current = document.documentElement.dataset.theme || 'light';
		applyTheme(current === 'dark' ? 'light' : 'dark');
	}

	window.addEventListener('DOMContentLoaded', function () {
		applyTheme(getPreferredTheme());

		const btn = document.getElementById('mom-theme-toggle');
		if (btn && !btn.dataset.bound) {
			btn.addEventListener('click', function (e) {
				e.preventDefault();
				toggleTheme();
			});
			btn.dataset.bound = 'true';
		}
	});
})();
