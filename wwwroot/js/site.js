// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Highlight active navigation link based on current page
document.addEventListener('DOMContentLoaded', function() {
    const currentPath = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    
    navLinks.forEach(link => {
        const linkPath = link.getAttribute('href').toLowerCase();
        
        // Check if current path matches the link path
        if (currentPath === linkPath || 
            (currentPath === '/' && linkPath.includes('/home/index')) ||
            (currentPath.includes('/home/about') && linkPath.includes('/home/about')) ||
            (currentPath.includes('/home/services') && linkPath.includes('/home/services')) ||
            (currentPath.includes('/home/contact') && linkPath.includes('/home/contact'))) {
            link.classList.add('active');
        }
    });
});