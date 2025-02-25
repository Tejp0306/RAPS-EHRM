function init() {
    
    const $ = go.GraphObject.make;

    const myDiagram = $(go.Diagram, "myDiagramDiv", {
        initialAutoScale: go.Diagram.Uniform,
        "undoManager.isEnabled": true,
        layout: $(go.TreeLayout, { angle: 90, layerSpacing: 35 }) // Hierarchical layout
    });

    myDiagram.nodeTemplate =
        $(go.Node, "Auto",
            $(go.Shape, "RoundedRectangle", { fill: "#009CFF", strokeWidth: 0 }),
            $(go.Panel, "Table",
                $(go.TextBlock,
                    { row: 0, margin: 8, font: "bold 12px sans-serif" },
                    new go.Binding("text", "name")
                ),
                $(go.TextBlock,
                    { row: 1, margin: 5, font: "10px sans-serif", stroke: "white" },
                    new go.Binding("text", "title")
                )
            )
        );

    myDiagram.linkTemplate =
        $(go.Link,
            $(go.Shape, { strokeWidth: 1, stroke: "black" })
        );

    // Fetch hierarchical data from the server
    jQuery.ajax({
        
        url: '/Hierarchy/ShowHierarchy',  // Update this based on your controller method
        method: 'GET',
        dataType: 'JSON',
        success: function (data) {
            console.log(data);
            
            if (Array.isArray(data) && data.length > 0) {
                myDiagram.model = new go.TreeModel(data);
   
            } else {
                console.warn("No data received or incorrect format");
            }
        },
        error: function (xhr, status, error) {
            console.error("Error fetching hierarchy data:", error);
            alert("Error loading organization chart data.");
        }
    });
}

// Ensure init() is called after the DOM is ready
$(document).ready(function () {
    init();
});
