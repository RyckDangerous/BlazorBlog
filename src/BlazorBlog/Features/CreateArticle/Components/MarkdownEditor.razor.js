window.insertMarkdown = function(textAreaId, before, after) {
    const textarea = document.getElementById(textAreaId);
    if (!textarea) {
        return;
    }
    
    const start = textarea.selectionStart;
    const end = textarea.selectionEnd;
    const selectedText = textarea.value.substring(start, end);
    const textBefore = textarea.value.substring(0, start);
    const textAfter = textarea.value.substring(end);
    
    // Construire le nouveau texte
    let newText;
    let newCursorPos;
    if (selectedText) {
        // Si du texte est sélectionné, l'entourer avec before et after
        newText = textBefore + before + selectedText + after + textAfter;
        newCursorPos = start + before.length + selectedText.length + after.length;
    } else {
        // Sinon, insérer before et after, et placer le curseur entre les deux
        newText = textBefore + before + after + textAfter;
        newCursorPos = start + before.length;
    }
    
    textarea.value = newText;
    
    // Repositionner le curseur
    textarea.setSelectionRange(newCursorPos, newCursorPos);
    textarea.focus();
    
    // Déclencher l'événement input pour mettre à jour le binding Blazor
    const inputEvent = new Event('input', { bubbles: true, cancelable: true });
    textarea.dispatchEvent(inputEvent);
};

