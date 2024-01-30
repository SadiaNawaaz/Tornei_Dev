window.MiaFinestraDiDialogo = function () {
    document.getElementById('mio-dialogo').showModal()
}

/*Permette l'imput di soli numeri interi e decimali*/
/*Allows the entry of whole numbers and decimals only*/
function SoloNumeriDecimali(Evento) {

    var Carattere = (Evento.which) ? Evento.which : Evento.keyCode;
    if (Carattere > 31 && (Carattere < 48 || Carattere > 57) && Carattere != 44) {
        return false;
    }
    return true;
}

/*Permette l'imput di soli numeri interi decimali*/
/*Allows input of decimal integers only. In Italy, it is the decimal separator*/
function SoloNumeriInteri(Evento) {
    var Carattere = (Evento.which) ? Evento.which : Evento.keyCode;
    if (Carattere > 31 && (Carattere < 48 || Carattere > 57)) {
        return false;
    }
    return true;
}



// Procedura per la gestione dei menù
// INIZIO
var clickedNodeId;

// Seleziona l'elemento cliccato
// Credo sia una funzione termporanea da usare in fase di progettazione
function setClickedNodeId(nodeId) {
    console.log("Clicked Node ID:", nodeId);
    clickedNodeId = nodeId;
    alert("Clicked Node ID: " + nodeId);
}

// Mostra il menù selezionato
function showContextMenu(x, y) {

    var contextMenu = document.getElementById('context-menu');
    contextMenu.style.display = 'block';
    contextMenu.style.left = x + 'px';
    contextMenu.style.top = y + 'px';
}

// Nasconde il menù selezionato

function hideContextMenu() {
    var contextMenu = document.getElementById('context-menu');
    contextMenu.style.display = 'none';
}

document.addEventListener('contextmenu', function (event) {
    // Prevent the default context menu
    event.preventDefault();

    // Trigger a left click event at the same position
    var leftClickEvent = new MouseEvent('click', {
        bubbles: true,
        cancelable: true,
        clientX: event.clientX,
        clientY: event.clientY
    });

    // Dispatch the left click event
    event.target.dispatchEvent(leftClickEvent);
    showContextMenu(event.clientX, event.clientY);
});


document.addEventListener('click', function () {
    hideContextMenu();
});

// Restituisce il codice del menù selezionato
function getClickedNodeId() {
    console.log("Clicked Node ID:", clickedNodeId);
    return clickedNodeId !== undefined && clickedNodeId !== null ? parseInt(clickedNodeId, 10) : 0;
}
// FINE