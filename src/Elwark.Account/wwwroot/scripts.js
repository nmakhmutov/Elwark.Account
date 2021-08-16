const language = localStorage['ls'];
(async function () {
    await Blazor.start({applicationCulture: language ? JSON.parse(language) : 'en'});
})();
