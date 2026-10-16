window.intersectionObserver = {
    observers: {},
    register: function (dotNetObjRef, id) {
        // Ruim een eventuele oude observer voor deze id direct op
        if (this.observers[id]) {
            this.observers[id].disconnect();
        }

        const el = document.getElementById(id);
        if (!el) {
            // Als Blazor het element nog niet in de DOM heeft gezet, probeer het een fractie later opnieuw
            setTimeout(() => this.register(dotNetObjRef, id), 50);
            return;
        }

        const observer = new IntersectionObserver(entries => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    dotNetObjRef.invokeMethodAsync('OnIntersect')
                        .catch(err => console.error("OnIntersect failed:", err));
                }
            });
        }, {
            root: null, // window viewport
            rootMargin: '100px', // Trigger al 100px VOORDAT de gebruiker de bodem bereikt (scrollt veel soepeler!)
            threshold: 0
        });

        observer.observe(el);
        this.observers[id] = observer;
    },
    unregister: function (id) {
        if (this.observers[id]) {
            this.observers[id].disconnect();
            delete this.observers[id];
        }
    },
    ensureVisible: function (dotNetObjRef, id) {
        const el = document.getElementById(id);
        if (el) {
            const rect = el.getBoundingClientRect();
            // Als de sentinel zich boven of in het zichtbare scherm bevindt
            if (rect.top <= window.innerHeight) {
                dotNetObjRef.invokeMethodAsync('OnIntersect')
                    .catch(err => console.error("ensureVisible OnIntersect failed:", err));
            }
        }
    }
};
