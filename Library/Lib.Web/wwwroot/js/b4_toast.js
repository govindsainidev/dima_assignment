
const toastContainer = document.querySelector('.toast-placement-ex');
let selectedType, selectedPlacement, toastPlacement;

const toastType = Object.freeze({
    primary: "bg-primary",
    secondary: "bg-secondary",
    success: "bg-success",
    danger: "bg-danger",
    warning: "bg-warning",
    info: "bg-info",
    dark: "bg-dark",
})

const toastPlacementObj = Object.freeze({
    top_left: ["top-0", "start-0"],
    top_center: ["top-0 start-50", "translate-middle-x"],
    top_right: ["top-0", "end-0"],
    middle_left: ["top-50 start-0", "translate-middle-y"],
    middle_center: ["top-50 start-50", "translate-middle"],
    middle_right: ["top-50 end-0", "translate-middle-y"],
    bottom_left: ["bottom-0", "start-0"],
    bottom_center: ["bottom-0", "start-50 translate-middle-x"],
    bottom_right: ["bottom-0", "end-0"],
})

var toastDispose = (toast) => {
    if (toast && toast._element !== null) {
        if (toastContainer) {
            toastContainer.classList.remove(selectedType);
            DOMTokenList.prototype.remove.apply(toastContainer.classList, selectedPlacement);
        }
        toast.dispose();
    }
}

var showToast = (title, text, type, placement) => {
    debugger;
    if (toastPlacement) {
        toastDispose(toastPlacement);
    }

    let toastTitle = document.querySelector('#toastTitle'),
        toastTime = document.querySelector('#toastTime'),
        toastMessage = document.querySelector('#toastMessage');

    if (typeof title !== "undefined" && title)
        toastTitle.innerHTML = title;
    else
        toastTitle.innerHTML = "Message";

    if (typeof text !== "undefined" && text)
        toastMessage.innerHTML = text;
    else
        toastMessage.innerHTML = "A simple Toast Message";

    if (typeof placement !== "undefined" && placement)
        selectedPlacement = placement;
    else
        selectedPlacement = "bottom-0 end-0";

    if (typeof type !== "undefined" && type)
        selectedType = type;
    else
        selectedType = toastType.dark;


    toastContainer.classList.add(selectedType);
    DOMTokenList.prototype.add.apply(toastContainer.classList, selectedPlacement);
    toastPlacement = new bootstrap.Toast(toastContainer);
    toastPlacement.show();

}

var showWarningToast = (text) => {
    if (typeof text === "undefined")
        text = "Error! Oops Something wrong";

    showToast("Warning", text, toastType.danger, toastPlacementObj.bottom_right)
}

var showSuccessToast = (text) => {

    showToast("Success", text, toastType.success, toastPlacementObj.bottom_right)
}

var showInfoToast = (text) => {
    showToast("Info", text, toastType.info, toastPlacementObj.bottom_right)
}


