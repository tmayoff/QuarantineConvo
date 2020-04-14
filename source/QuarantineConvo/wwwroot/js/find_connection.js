$(document).ready(() => {

    $(".interest-checkbox").on("change", (e) => {
        let checkbox = $(e.currentTarget)
        let checked = checkbox.is(':checked')
        if (checked) {
            checkbox.parent().addClass("interest-checked")
        } else {
            checkbox.parent().removeClass("interest-checked")
        }
    })

})