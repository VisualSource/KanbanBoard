/// <reference types="../../src/types/htmx"/>
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
/**
 * @param {boolean} state
 * @param {HTMLButtonElement|undefined} target
 */
function updateCount(target, state) {
  if (!target) return;
  /**
   * @type {HTMLHeadingElement}
   */
  const header = target.parentElement.parentElement.previousElementSibling;
  if (!header) return;
  const done = parseInt(header.getAttribute("data-done") ?? "0");
  const total = parseInt(header.getAttribute("data-total") ?? "0");

  const next = done + state ? 1 : -1;
  header.setAttribute("data-done", next.toString());
  header.innerText = `Subtasks (${next} of ${total})`;
}

function closeModel(dialog) {
  /** @type {HTMLDialogElement} */
  const target =
    dialog instanceof HTMLDialogElement
      ? dialog
      : document.getElementById(dialog);
  if (!target) return;

  target.close();
  /**
   * @type {HTMLFormElement}
   */
  const formEl = target.firstElementChild;
  formEl.reset();
}

(function () {
  const prefersDarkScheme = window.matchMedia("(prefers-color-scheme: dark)");
  if (prefersDarkScheme.matches) {
    document.body.classList.add("dark");
  }

  window.addEventListener("click", (ev) =>
    window.dispatchEvent(
      new CustomEvent("click-away", { detail: { target: ev.target } })
    )
  );
  class ItemList extends HTMLDivElement {
    static formAssociated = true;

    /**
     * @type {HTMLButtonElement|null}
     * @memberof ItemList
     */
    addBtnElement = null;
    /**
     * @type {HTMLTemplateElement|null}
     * @memberof ItemList
     */
    itemTemplete = null;
    /**
     * @type {HTMLUListElement|null}
     * @memberof ItemList
     */
    itemsContainer = null;
    constructor() {
      super();
    }
    get name() {
      return "SubTasks";
    }
    get value() {
      let data = new FormData();
      const len = this.itemsContainer.children.length;
      for (let i = 0; i < len; i++) {
        const el = this.itemsContainer.children.item(i).querySelector("input");
        if (!el) continue;

        const state = el.getAttribute("data-state") ?? false;
        data.append(`${this.name}[${i}].State`, state ? "true" : "false");

        const id = el.getAttribute("data-id") ?? undefined;
        if (id) {
          data.append(`${this.name}[${i}].Id`, id);
        }

        data.append(`${this.name}[${i}].Title`, el.value);
      }

      return data;
    }
    onAddItem = () => {
      /** @type {HTMLLIElement} */
      const clone = this.itemTemplete.content.cloneNode(true);

      clone
        .querySelector("button")
        .addEventListener("click", (ev) => ev.target.parentElement.remove());
      this.itemsContainer.appendChild(clone);
    };
    connectedCallback() {
      this.addBtnElement = this.querySelector("button[data-add-btn]");
      this.itemTemplete = this.querySelector("template[data-item]");
      this.itemsContainer = this.querySelector("ul[data-items]");
      if (!this.addBtnElement) throw new Error("Missing add btn.");

      this.addBtnElement.addEventListener("click", this.onAddItem);

      for (const dialog of document.querySelectorAll("dialog") ?? []) {
        if (dialog.contains(this)) {
          const form = dialog.firstElementChild;
          form.addEventListener(
            "reset",
            () => (this.itemsContainer.innerHTML = "")
          );
          break;
        }
      }
    }
  }

  customElements.define("item-list", ItemList, { extends: "div" });
  class DropDown extends HTMLDivElement {
    static observedAttributes = ["data-state"];
    /**
     * @type {HTMLElement|null}
     *
     * @memberof DropDown
     */
    triggerElement = null;
    /**
     * @type {HTMLElement|null}
     *
     * @memberof DropDown
     */
    menuElement = null;

    constructor() {
      super();
      window.addEventListener("click-away", this.handleAwayClick);
    }
    /**
     * @linkcode https://github.com/alpinejs/alpine/blob/main/packages/alpinejs/src/utils/on.js#L42C11-L43C11
     * @param {CustomEvent<{ target: HTMLElement }>} ev
     */
    handleAwayClick = (ev) => {
      if (this.contains(ev.detail.target)) return;
      if (!ev?.detail?.target.isConnected) return;
      if (this.offsetWidth < 1 && this.offsetHeight < 1) return;
      if (this.getAttribute("data-state") !== "open") return;
      this.setAttribute("data-state", "closed");
    };

    handleClick = () => {
      const state = this.getAttribute("data-state") ?? "closed";
      this.setAttribute("data-state", state === "open" ? "closed" : "open");
    };

    connectedCallback() {
      const triggerElement = this.querySelector("[data-trigger]");
      if (!triggerElement) {
        throw new Error(
          "Missing Trigger element. Add data-trigger to element."
        );
      }

      triggerElement.addEventListener("click", this.handleClick);
      this.triggerElement = triggerElement;

      const menuElement = this.querySelector("[data-content]");
      if (!menuElement) {
        throw new Error(
          "Missing Menu element. Add 'data-content' to a element."
        );
      }
      this.menuElement = menuElement;

      this.setAttribute("data-state", "closed");
    }
    disconnectedCallback() {
      this.triggerElement?.removeEventListener("click", this.handleClick);
      window.removeEventListener("click-away", this.handleAwayClick);
    }
    attributeChangedCallback(name, oldValue, newValue) {
      if (name !== "data-state") return;
      switch (newValue) {
        case "open":
          this.menuElement?.classList.remove("hidden");
          break;
        case "closed":
          this.menuElement?.classList.add("hidden");
          break;
        default:
          break;
      }
    }
  }

  customElements.define("dropdown-menu", DropDown, { extends: "div" });

  htmx.defineExtension("include-custom-input", {
    onEvent(name, evt) {
      if (name !== "htmx:configRequest") return true;
      /** @type {HTMLElement} */
      const sourceEl = evt.detail.elt;
      const targetEl = sourceEl.getAttribute("include-input");
      if (!targetEl) return true;

      const el = htmx.find(targetEl);
      if (!el) return true;
      /**
       * @type {FormData}
       */
      const content = el.value;

      for (const [key, v] of content.entries()) {
        evt.detail.parameters[key] = v;
      }
    },
  });

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
          this.option("disabled", true);
        },
      });

      // Re-enable sorting on the `htmx:afterSwap` event
      sortable.addEventListener("htmx:afterSwap", function () {
        sortableInstance.option("disabled", false);
      });
    }
  });
})();
