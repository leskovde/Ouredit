const { ipcRenderer } = require("electron");

/**
 * Removes the 'selected' style from every file tab and applies it to the given argument.
 * A confirmation message is sent to the main process via the 'async-tab-select-js-caller' channel.
 * @param {any} elem The element which will be set to selected.
 */
function changeToSelected(elem) {
    if (elem.classList.contains("selected")) {
        return;
    }

    const tabsContainer = document.getElementById("files-ribbon");

    for (const child of tabsContainer.children) {
        child.classList.remove("selected");
    }

    elem.classList.add("selected");


    const textContent = elem.innerHTML;
    ipcRenderer.send("async-tab-select-js-caller", textContent);
}

/**
 * Upon receiving a message through the 'async-tab-select-cs-caller', a file tab element specified by
 * the innerHTML text will be set to the 'selected' style. If no such element exists, a new one is created.
 */
ipcRenderer.on("async-tab-select-cs-caller", (event, arg) => {
    const tabsContainer = document.getElementById("files-ribbon");

    for (const child of tabsContainer.children) {
        const textContent = child.innerHTML;


        if (textContent === arg) {
            changeToSelected(child);
            return;
        }
    }

    var elem = document.createElement("button");
    elem.classList.add("file-tab");
    elem.innerHTML = arg;
    elem.setAttribute("onclick", "changeToSelected(this)");

    tabsContainer.appendChild(elem);
    changeToSelected(elem);
});
