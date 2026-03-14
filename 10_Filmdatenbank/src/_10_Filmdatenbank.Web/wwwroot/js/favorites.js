document.addEventListener('DOMContentLoaded', () => {
    const favoriteButtons = document.querySelectorAll('.favorite-toggle');
    const toastContainer = document.getElementById('toast-container');

    favoriteButtons.forEach(button => {
        button.addEventListener('click', async (e) => {
            e.preventDefault();
            e.stopPropagation();

            const filmId = button.dataset.filmId;
            const url = button.dataset.url;
            const icon = button.querySelector('i');

            try {
                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'X-Requested-With': 'XMLHttpRequest',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                });

                if (response.ok) {
                    const data = await response.json();
                    if (data.success) {
                        // Update Icon
                        if (data.isAdded) {
                            icon.classList.remove('fa-regular', 'text-white');
                            icon.classList.add('fa-solid', 'text-danger-primary');
                        } else {
                            icon.classList.remove('fa-solid', 'text-danger-primary');
                            icon.classList.add('fa-regular', 'text-white');
                        }

                        // Show Toast
                        showToast(data.message, data.isAdded ? 'success' : 'info');
                    }
                } else if (response.status === 401) {
                    window.location.href = '/Identity/Account/Login';
                }
            } catch (error) {
                console.error('Error toggling favorite:', error);
                showToast('Ein Fehler ist aufgetreten.', 'error');
            }
        });
    });

    function showToast(message, type) {
        if (!toastContainer) return;

        const toast = document.createElement('div');
        toast.className = `toast-item pointer-events-auto flex items-center gap-4 px-6 py-4 rounded-2xl bg-surface-bright/80 dark:bg-dark-surface-soft/80 backdrop-blur-xl border border-${type === 'success' ? 'success' : type === 'error' ? 'danger' : 'success'}-primary/20 shadow-2xl shadow-${type === 'success' ? 'success' : type === 'error' ? 'danger' : 'success'}-primary/10 transform transition-all duration-500 translate-y-20 opacity-0`;
        toast.setAttribute('role', 'alert');
        toast.dataset.type = type;

        const iconClass = type === 'success' ? 'fa-check' : type === 'error' ? 'fa-circle-exclamation' : 'fa-info-circle';
        const bgClass = type === 'success' ? 'bg-success-primary' : type === 'error' ? 'bg-danger-primary' : 'bg-success-primary';
        const labelText = type === 'success' ? 'Erfolg' : type === 'error' ? 'Fehler' : 'Info';
        const labelColor = type === 'success' ? 'text-success-content dark:text-success-dark-content' : type === 'error' ? 'text-danger-content dark:text-danger-dark-content' : 'text-success-content dark:text-success-dark-content';

        toast.innerHTML = `
            <div class="w-10 h-10 rounded-full ${bgClass} flex items-center justify-center text-white shrink-0 shadow-lg shadow-${type}-primary/20">
                <i class="fa-solid ${iconClass}"></i>
            </div>
            <div class="flex flex-col">
                <span class="text-[10px] font-black ${labelColor} uppercase tracking-widest">${labelText}</span>
                <span class="text-sm font-bold text-text-main dark:text-white">${message}</span>
            </div>
            <button onclick="this.parentElement.remove()" class="ml-4 text-text-muted hover:text-text-main dark:hover:text-white transition-colors">
                <i class="fa-solid fa-xmark"></i>
            </button>
        `;

        toastContainer.appendChild(toast);

        // Animate in
        setTimeout(() => {
            toast.classList.remove('translate-y-20', 'opacity-0');
            toast.classList.add('translate-y-0', 'opacity-100');
        }, 10);

        // Auto-hide
        setTimeout(() => {
            toast.classList.add('translate-y-20', 'opacity-0');
            setTimeout(() => toast.remove(), 500);
        }, 5000);
    }
});
