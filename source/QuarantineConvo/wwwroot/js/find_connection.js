let notifyTemplate = `
<div class="bg-light shadow-lg d-flex flex-row align-items-start" data-notify="container">
    <div class="d-flex flex-column flex-grow-1" >
        <h3 data-notify="title">{1}</h3>
        </div >
    <button class="btn d-xl-flex" type="button" data-notify="dismiss"><i class="fas fa-times"></i></button>
</div >`

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

    $("#submit_button").on("click", (e) => {
        e.preventDefault()

        console.log("Processing Request")

        var interests = [];
        $.each($("input[name='interestCheckboxes']:checked"), function () {
            interests.push($(this).val());
        });

        console.log("Received Interests: " + interests.join(", "))

        $.post("/Messaging/Search", { interestCheckboxes: interests })
            .done((data, status, jqXHR) => {
                console.log("Data: " + data)

                if (data != "EMPTY") {
                    $.notify(
                        {
                            title: `<strong>${data}</strong>`
                        },
                        {
                            url_target: "_self",
                            placement: { from: "bottom", align: "right" },
                            mouse_over: "pause",
                            template: notifyTemplate
                        });
                }

            }).fail((jqXHR, status, error) => {
                console.error(error)
            })

        console.log("Request Sent")
    })
})