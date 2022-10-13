var input = document.getElementById('txtNumGaranzia');
function EnableDisableTextBox(chkGaranzia) {
	var txtNumGaranzia = document.getElementById("txtNumGaranzia");
	txtNumGaranzia.disabled = chkGaranzia.checked ? false : true;
	if (!txtNumGaranzia.disabled) {
		txtNumGaranzia.focus();
		input.setAttribute('required', '');
	}
	else {
		document.getElementById('txtNumGaranzia').value = "";
	}
			

}

