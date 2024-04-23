window.addEventListener("load", function () {
	document.title = "bürotime REST API Service";
	const linkElement1 = document.querySelector("link[sizes='32x32']");
	const linkElement2 = document.querySelector("link[sizes='16x16']");
	linkElement1.href = "https://www.burotime.com/Uploads/favicon.ico";
	linkElement2.href = "https://www.burotime.com/Uploads/favicon.ico";
	setTimeout(function () {
		const imgElement = document.querySelector(".link img");
		imgElement.src = "https://cdn.burotime.com//images/burotime-logo.png";
	}, 10);
});