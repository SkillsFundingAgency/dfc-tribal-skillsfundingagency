//
// Helper functions
//
//

function setUpShowHidePanels() {

    // map all h3 elements to a click event to show or hide the info
    var h3Elements, nextol, anchor, content;

    h3Elements = document.getElementsByTagName('h3');
    for (var i = 0; i < h3Elements.length; i++) {
        if (/showHide/.test(h3Elements[i].className)) {
            // get the adjacent span
            nextol = h3Elements[i].nextSibling;
            while (nextol.nodeType !== 1)
                nextol = nextol.nextSibling;

            // hide it
            nextol.style.display = 'none';
            //create a new link
            anchor = document.createElement('a');
            // copy original showLink text and add attributes
            content = document.createTextNode(h3Elements[i].firstChild.nodeValue);
            anchor.appendChild(content);
            anchor.href = '#show';
            anchor.title = 'Click to open.';
            anchor.nextol = nextol;
            anchor.onclick = function () { showHide(this.nextol); changeTitle(this); return false }
            // replace span with created link
            h3Elements[i].replaceChild(anchor, h3Elements[i].firstChild);
        }
    }
}


// hide / unhide data entry blocks
function showHide(h3Element) {
    if (h3Element)
        h3Element.style.display = h3Element.style.display === 'none' ? 'block' : 'none';
}

// change title based on whether the panel is expanded or closed
function changeTitle(h3Element) {
    if (h3Element)
        h3Element.title = h3Element.title === 'Click to open.' ? 'Click to hide.' : 'Click to open.';
}