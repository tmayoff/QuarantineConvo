
$(document).ready(() => {

    // Sidebar stuff
    $(".sidebar-toggle").on("click", () => {
        ToggleSidebar();
    })

})

function ToggleSidebar() {
    let sidebar = $(".sidebar")
    sidebar.toggleClass("active");
}