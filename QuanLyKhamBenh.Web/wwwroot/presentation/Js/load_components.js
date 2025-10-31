document.addEventListener('DOMContentLoaded', () => {
    const loadComponent = async (placeholderId, url) => {
        try {
            const response = await fetch(url);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.text();
            document.getElementById(placeholderId).innerHTML = data;
        } catch (error) {
            console.error(`Error loading component from ${url}:`, error);
        }
    };

    // Load header and footer
    loadComponent('header-placeholder', 'Share/_header.html');
    loadComponent('footer-placeholder', 'Share/_footer.html');
});
