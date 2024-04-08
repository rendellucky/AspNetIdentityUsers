var myModal = new bootstrap.Modal(document.getElementById('my-modal'), {
    keyboard: false
})

function loadModalContent(url) {
    fetch(url).then(r => r.text()).then(html => {
        setModalContent(html)
    })
}

function setModalContent(html) {
    let modalContent = $("#my-modal .modal-dialog")
    modalContent.empty()
    modalContent.html(html)
    myModal.show()
}

$(document).on('click', "a.open-in-modal", (e) => {
    e.preventDefault()
    let el = $(e.target)
    let url = el.attr("href")
    loadModalContent(url)
})

$(document).on('click', "button.open-in-modal", (e) => {
    let el = $(e.target)
    let url = el.attr("formaction")
    loadModalContent(url)
})

$(document).on('submit', "form.ajax-form", async e => {
	e.preventDefault()

	let url = $(e.target).attr('action')
	let data = $(e.target).serializeArray()

	let formData = new FormData()
	data.forEach(x => {
		formData.append(x.name, x.value)
	})

	await fetch(url, {
		method: "POST",
		body: formData
	}).then(r => {
		if (r.redirected) {
			window.location.reload()
		} else {
			return r.text()
		}
	}).then(html => {
		setModalContent(html)
	})
})