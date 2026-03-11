/* Dark Mode Toggle Logic */
(function () {
    const getTheme = () => {
        const stored = localStorage.getItem('theme-preference');
        if (stored) return stored;
        return 'system';
    };

    const applyTheme = (theme) => {
        const isDark = theme === 'dark' || 
            (theme === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches);
        
        if (isDark) {
            document.documentElement.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
        }
    };

    const init = () => {
        const themeRadios = document.querySelectorAll('input[name="theme-toggle"]');
        const currentTheme = getTheme();

        // Initial apply
        applyTheme(currentTheme);

        // Update UI state
        themeRadios.forEach(radio => {
            if (radio.value === currentTheme) {
                radio.checked = true;
            }

            radio.addEventListener('change', (e) => {
                const newTheme = e.target.value;
                localStorage.setItem('theme-preference', newTheme);
                applyTheme(newTheme);
            });
        });

        // Listen for system changes
        window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
            if (getTheme() === 'system') {
                applyTheme('system');
            }
        });
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
