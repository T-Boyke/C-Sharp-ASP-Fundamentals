/* Dark Mode Toggle Logic */
(function () {
    // 1. Theme determination logic
    const getTheme = () => {
        return localStorage.getItem('theme-preference') || 'system';
    };

    const applyTheme = (theme) => {
        const isDark = theme === 'dark' || 
            (theme === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches);
        
        if (isDark) {
            document.documentElement.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
        }
        // Dispatch event for any other listeners
        window.dispatchEvent(new CustomEvent('theme-changed', { detail: { theme, isDark } }));
    };

    // 2. Immediate execution to prevent FOUC (Flash of Unstyled Content)
    // This part runs as soon as the script is loaded in the <head>
    const initialTheme = getTheme();
    applyTheme(initialTheme);

    // 3. UI Initialization logic (runs after DOM is ready)
    const init = () => {
        const themeRadios = document.querySelectorAll('input[name="theme-toggle"]');
        if (themeRadios.length === 0) return;

        const currentTheme = getTheme();

        // Update UI state to match stored preference
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

        // Listen for system preference changes
        const darkMediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
        try {
            // Modern browsers
            darkMediaQuery.addEventListener('change', () => {
                if (getTheme() === 'system') applyTheme('system');
            });
        } catch (e) {
            // Deprecated callback for older browsers
            darkMediaQuery.addListener(() => {
                if (getTheme() === 'system') applyTheme('system');
            });
        }
    };

    // Set up initializer
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
