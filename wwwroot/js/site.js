// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    // Фільтрація за категорією
    document.querySelectorAll('.category-filter').forEach(button => {
        button.addEventListener('click', function () {
            const categoryId = this.dataset.categoryId;
            window.location.href = `/Product?categoryId=${categoryId}`;
        });
    });

    // Фільтрація за підкатегорією
    document.querySelectorAll('.subcategory-filter').forEach(button => {
        button.addEventListener('click', function () {
            const subcategoryId = this.dataset.subcategoryId;
            window.location.href = `/Product?subcategoryId=${subcategoryId}`;
        });
    });

    // Пошук
    const searchForm = document.getElementById('search-form');
    if (searchForm) {
        searchForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const query = document.getElementById('search-input').value;
            window.location.href = `/Product?search=${encodeURIComponent(query)}`;
        });
    }
});
