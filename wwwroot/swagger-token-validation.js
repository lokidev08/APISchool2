(function () {
    const validationUrl = "/Login/validate-token";
    const schemeName = "Bearer";
    const authorizedClicks = new WeakSet();

    function normalizeToken(value) {
        return (value || "").trim().replace(/^Bearer\s+/i, "");
    }

    function findTokenInput(button) {
        const root = button.closest(".dialog-ux") || document;
        const inputs = Array.from(root.querySelectorAll(
            ".auth-container input[type='text'], .auth-container input[type='password'], input[type='text'], input[type='password']"
        ));

        return inputs.find(input => (input.value || "").trim().length > 0) || inputs[0] || null;
    }

    function showMessage(input, message, isValid) {
        const root = input.closest(".auth-container") || input.parentElement;
        if (!root) {
            return;
        }

        let messageElement = root.querySelector(".jwt-validation-message");
        if (!messageElement) {
            messageElement = document.createElement("p");
            messageElement.className = "jwt-validation-message";
            messageElement.style.margin = "8px 0 0";
            messageElement.style.fontSize = "13px";
            messageElement.style.fontWeight = "600";
            root.appendChild(messageElement);
        }

        messageElement.textContent = message;
        messageElement.style.color = isValid === true
            ? "#15803d"
            : isValid === false
                ? "#b91c1c"
                : "#475569";
    }

    function clearSwaggerAuthorization() {
        try {
            if (window.ui && window.ui.authActions && window.ui.authActions.logout) {
                window.ui.authActions.logout([schemeName]);
            }
        } catch {
            // Swagger UI may not be fully initialized yet.
        }
    }

    function isAuthorizeButton(button) {
        if (!button) {
            return false;
        }

        const text = (button.textContent || "").trim().toLowerCase();
        const isInAuthorizeDialog = button.closest(".dialog-ux") || button.closest(".auth-container");

        return text === "authorize" && isInAuthorizeDialog;
    }

    document.addEventListener("click", async function (event) {
        const button = event.target.closest ? event.target.closest("button") : null;
        if (!isAuthorizeButton(button)) {
            return;
        }

        if (authorizedClicks.has(button)) {
            authorizedClicks.delete(button);
            return;
        }

        const input = findTokenInput(button);
        if (!input) {
            return;
        }

        event.preventDefault();
        event.stopPropagation();
        event.stopImmediatePropagation();

        const token = normalizeToken(input.value);
        if (!token) {
            const message = "Debes escribir un token.";
            clearSwaggerAuthorization();
            showMessage(input, message, false);
            alert(message);
            return;
        }

        button.disabled = true;
        showMessage(input, "Validando token...", null);

        try {
            const response = await fetch(validationUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ token })
            });

            const result = await response.json().catch(function () {
                return {};
            });

            const message = result.message || (
                response.ok
                    ? "Token valido. Ya estas autorizado."
                    : "Token invalido. No estas autorizado."
            );

            if (!response.ok || result.valid !== true) {
                clearSwaggerAuthorization();
                showMessage(input, message, false);
                alert(message);
                return;
            }

            input.value = token;
            showMessage(input, message, true);
            alert(message);

            button.disabled = false;
            authorizedClicks.add(button);
            button.click();
        } catch {
            const message = "No se pudo validar el token.";
            clearSwaggerAuthorization();
            showMessage(input, message, false);
            alert(message);
        } finally {
            button.disabled = false;
        }
    }, true);
})();
