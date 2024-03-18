myFunction = () => {
    var element = document.body;
    element.classList.toggle("dark-mode");
    var img = element.querySelector(".lightmode")
    img.src.includes("darkmode") ? img.src = "../../images/lightmode.png" : img.src = "../../images/darkmode.png";
}
