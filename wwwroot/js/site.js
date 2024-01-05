// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {
  document.getElementById("theme-switch").addEventListener("click", (ev) => {
    /** @type {HTMLButtonElement} */
    const target = ev.target;
    const state =
      target.getAttribute("data-state") === "unchecked"
        ? "checked"
        : "unchecked";
    target.setAttribute("data-state", state);
    target.firstElementChild.setAttribute("data-state", state);
    document.body.classList.toggle("dark");
  });
});

htmx.onLoad(function (content) {
  var sortables = content.querySelectorAll(".sortable");
  for (var i = 0; i < sortables.length; i++) {
    var sortable = sortables[i];
    const groupName = sortable.getAttribute("data-group-name") ?? undefined;
    var sortableInstance = new Sortable(sortable, {
      animation: 150,
      group: groupName,
      ghostClass: "blue-background-class",

      // Make the `.htmx-indicator` unsortable
      filter: ".htmx-indicator, .non-sort",
      onMove: function (evt) {
        return evt.related.className.indexOf("htmx-indicator") === -1;
      },

      // Disable sorting on the `end` event
      onEnd: function (evt) {
        //this.option("disabled", true);
      },
    });

    // Re-enable sorting on the `htmx:afterSwap` event
    sortable.addEventListener("htmx:afterSwap", function () {
      //sortableInstance.option("disabled", false);
    });
  }
});
