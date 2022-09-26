

$(document).ready(function () {
	$('.dropdown-select').select2({
		allowClear: true,
		theme: 'bootstrap-5'
	});


	let roleInput = document.querySelector("#Role");
	let noOfEmpInput = document.querySelector(".NoOfEmployees");
	let dateOfEstInput = document.querySelector(".DateOfEstablishment");

	roleInput.addEventListener("change", function () {
		if (roleInput.value == "Изведувач") {
			noOfEmpInput.classList.remove("d-none");
			dateOfEstInput.classList.remove("d-none");
		}
		else {
			noOfEmpInput.classList.add("d-none")
			dateOfEstInput.classList.add("d-none")

			document.querySelector("#NoOfEmployees").value = null;
			document.querySelector("#DateOfEstablishment").value = null;
        }
    })
});