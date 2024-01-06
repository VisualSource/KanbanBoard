// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/**
 * Global Toggle Function for working with shadcn-ui components using data-state attributes.
 *
 * @param {[HTMLElement, ...HTMLElement[]]} target
 * @param {(state: boolean)=>void | undefined} callback
 */
function checkToggle(targets, callback) {
  if (!targets || !Array.isArray(targets) || !targets[0]) return;

  const state =
    targets[0].getAttribute("data-state") === "unchecked"
      ? "checked"
      : "unchecked";

  for (const item of targets) item.setAttribute("data-state", state);

  if (callback) callback(state === "checked");
}

function showModel(dialogName) {
  document.getElementById(dialogName)?.showModal();
}

function closeModel(dialogName) {
  document.getElementById(dialogName)?.close();
}

(function () {
  window.addEventListener("click", () =>
    window.dispatchEvent(new CustomEvent("click-away"))
  );
  class DropDown extends HTMLDivElement {
    constructor() {
      super();
      this.triggerElement = null;
      this.menuElement = null;

      window.addEventListener("click-away", this.onWindowEvent);
    }

    onWindowEvent = (ev) => {
      ev.preventDefault();
      console.log("Hello w");
      this.menuElement?.classList.remove("hidden");
    };
    onClickEvent = (ev) => {
      ev.preventDefault();
      console.log("HELLO");
      console.log(this.menuElement);
      this.menuElement?.classList.toggle("hidden");
    };

    connectedCallback() {
      this.triggerElement = this.querySelector("[data-trigger]");
      this.menuElement = this.querySelector("[data-content]");
      this.triggerElement.addEventListener("click", this.onClickEvent);
    }
    disconnectedCallback() {
      this.triggerElement.removeEventListener("click", this.onClickEvent);
      window.removeEventListener("click-away", this.onWindowEvent);
    }
  }

  customElements.define("dropdown-menu", DropDown, { extends: "div" });

  htmx.onLoad(function (content) {
    /** Setup drag and drop lib */
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
})();
