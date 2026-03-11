/* c:\Users\Tobia\Desktop\cSharpRepo\C-Sharp-ASP-Fundamentals\10_Filmdatenbank\src\_10_Filmdatenbank.Web\wwwroot\js\darkmode.js */
document.addEventListener('DOMContentLoaded', () => {
    const themeRadios = document.querySelectorAll('input[name="theme-toggle"]');
    
    // Check initial logic
    const getTheme = () => {
        if (localStorage.getItem('theme-preference')) {
            return localStorage.getItem('theme-preference');
        }
        return 'system';
    };

    const applyTheme = (theme) => {
        if (theme === 'dark' || (theme === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
            document.documentElement.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
        }
    };

    const setTheme = (theme) => {
        localStorage.setItem('theme-preference', theme);
        applyTheme(theme);
        
        // Update UI
        themeRadios.forEach(radio => {
            radio.checked = radio.value === theme;
        });
    };

    // Auto update for system preference changes
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        if (getTheme() === 'system') {
            applyTheme('system');
        }
    });

    // Event listeners for toggles
    themeRadios.forEach(radio => {
        radio.addEventListener('change', (e) => {
            setTheme(e.target.value);
        });
    });

    // Init UI from Storage
    setTheme(getTheme());
});
