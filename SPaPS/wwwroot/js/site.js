

$(document).ready(function () {
	$('.dropdown-select').select2({
		allowClear: true,
		theme: 'bootstrap-5'
	});


	let roleInput = document.querySelector("#Role");
	let noOfEmpInput = document.querySelector(".NoOfEmployees");
	let dateOfEstInput = document.querySelector(".DateOfEstablishment");

	if (roleInput) {
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
    }

	let serviceInput = document.querySelector("#ServiceId");

	if (serviceInput) {
		let buildingType = document.querySelector(".BuildingTypeId");
		let buildingSize = document.querySelector(".BuildingSize");
		let color = document.querySelector(".Color");
		let noOfWindows = document.querySelector(".NoOfWindows");
		let noOfDoors = document.querySelector(".NoOfDoors");

		serviceInput.addEventListener("change", function () {

			buildingType.classList.add("d-none");
			buildingSize.classList.add("d-none");
			color.classList.add("d-none");
			noOfWindows.classList.add("d-none");
			noOfDoors.classList.add("d-none");

			let serviceId = serviceInput.value;

			if (serviceId == null) {
				return;
            }

			if (serviceId == 3) {
				buildingType.classList.remove("d-none");
				noOfWindows.classList.remove("d-none");
				noOfDoors.classList.remove("d-none");
            }

			if (serviceId == 5) {
				buildingType.classList.remove("d-none");
				buildingSize.classList.remove("d-none");
				color.classList.remove("d-none");
			}

			if (serviceId == 8) {
				buildingType.classList.remove("d-none");
				buildingSize.classList.remove("d-none");
			}
		});

    }
});