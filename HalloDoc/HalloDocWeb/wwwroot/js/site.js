
myFunction = () => {
    var element = document.body;
    element.classList.toggle("dark-mode");
    var img = element.querySelector(".lightmode")
    img.src.includes("darkmode") ? img.src = "../../images/lightmode.png" : img.src = "../../images/darkmode.png";
}

<script>
    const input = document.querySelector("input[type='tel']");
    window.intlTelInput(input, {
        utilsScript: "https://cdn.jsdelivr.net/npm/intl-tel-input@19.2.16/build/js/utils.js",
    });
</script>