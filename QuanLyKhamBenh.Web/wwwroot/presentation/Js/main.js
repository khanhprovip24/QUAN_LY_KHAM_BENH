document.addEventListener('DOMContentLoaded', () => {
    const headerPlaceholder = document.getElementById('header-placeholder');
    const footerPlaceholder = document.getElementById('footer-placeholder');

    // --- LOAD HEADER & FOOTER ---
    const getSharePath = () => {
        const currentPath = window.location.pathname;
        // For files in Page directory (e.g., /GUI/Page/about_us.html)
        if (currentPath.includes('/Page/')) {
            return '../Share/';
        }
        // For files in Admin_page directory (e.g., /GUI/Admin_page/dashboard.html)
        if (currentPath.includes('/Admin_page/')) {
            return '../Share/';
        }
        // For files directly in GUI directory (e.g., /GUI/home.html, /GUI/admin.html)
        if (currentPath.includes('/GUI/')) {
            return 'Share/';
        }
        return 'Share/'; // Fallback
    };

    const sharePath = getSharePath();

    const loadComponents = async () => {
        // Load Header
        if (headerPlaceholder) {
            try {
                const response = await fetch(sharePath + '_header.html');
                const headerHtml = await response.text();
                headerPlaceholder.innerHTML = headerHtml;
            } catch (error) {
                console.error('Error loading header:', error);
                headerPlaceholder.innerHTML = '<p class="text-red-500 text-center">Lỗi tải header</p>';
            }
        }

        // Load Footer
        if (footerPlaceholder) {
            try {
                const response = await fetch(sharePath + '_footer.html');
                const footerHtml = await response.text();
                footerPlaceholder.innerHTML = footerHtml;
            } catch (error) {
                console.error('Error loading footer:', error);
                footerPlaceholder.innerHTML = '<p class="text-red-500 text-center">Lỗi tải footer</p>';
            }
        }

        // Sau khi tải xong header, chạy các script liên quan đến header
        initializeHeaderScripts();
        highlightActiveNav();
    };

    const initializeHeaderScripts = () => {
        const logoutButton = document.getElementById('logout-button');
        const welcomeUser = document.getElementById('welcome-user');

        // --- AUTHENTICATION ---
        const loggedInUser = sessionStorage.getItem('loggedInUser');
        if (!loggedInUser && !window.location.pathname.endsWith('login.html')) {
            window.location.href = 'login.html';
            return;
        }
        if (welcomeUser) {
            welcomeUser.textContent = `Chào, ${loggedInUser}!`;
        }

        if (logoutButton) {
            logoutButton.addEventListener('click', () => {
                sessionStorage.removeItem('loggedInUser');
                sessionStorage.removeItem('userRole');
                window.location.href = 'login.html';
            });
        }

        // --- DATE & TIME ---
        function updateDateTime() {
            const dateEl = document.getElementById('current-date');
            const timeEl = document.getElementById('current-time');
            if (!dateEl || !timeEl) return;
            const now = new Date();
            dateEl.textContent = now.toLocaleDateString('vi-VN', { weekday: 'long', day: 'numeric', month: 'numeric', year: 'numeric' });
            timeEl.textContent = now.toLocaleTimeString('vi-VN');
        }
        updateDateTime();
        setInterval(updateDateTime, 1000);

        // --- WEATHER API ---
        function fetchWeather() {
            const tempEl = document.getElementById('weather-temp');
            const iconEl = document.getElementById('weather-icon');
            if (!tempEl || !iconEl) return;
            
            // Dữ liệu thời tiết giả lập để tránh lỗi API và chạy nhanh hơn
            tempEl.textContent = '28°C';
            iconEl.textContent = '⛅️';
            console.log('Mock weather loaded.');
        }
        fetchWeather();
    };

    const highlightActiveNav = () => {
        const currentPage = window.location.pathname.split('/').pop();
        const navLinks = document.querySelectorAll('.nav-link');
        
        navLinks.forEach(link => {
            const linkPage = link.getAttribute('href');
            if (linkPage === currentPage || (currentPage === '' && linkPage === 'home.html')) {
                link.classList.add('text-blue-600', 'font-bold');
                link.classList.remove('text-gray-700');
            }
        });
    };

    // Bắt đầu quá trình
    loadComponents();
});