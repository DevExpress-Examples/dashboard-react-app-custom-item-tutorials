// #region imports
import { CustomItemViewer, ResourceManager } from 'devexpress-dashboard/common'
import { CustomItem } from 'devexpress-dashboard/model'
// #endregion
// #region svgIcon
var svgIcon = '<svg id="helloWorldItemIcon" viewBox="0 0 24 24"><path stroke="#42f48f" fill="#42f48f" d="M12 2 L2 22 L22 22 Z" /></svg>';
// #endregion
// #region metadata
var helloWorldItemMetaData = {
    customProperties: [{
        ownerType: CustomItem,
        propertyName: 'customProperty',
        valueType: 'string',
        defaultValue: 'Hello World!'
    }],
    optionsPanelSections: [{
        title: 'Custom Properties',
        items: [{
            dataField: 'customProperty',
            label: {
                text: 'Item Text'
            },
            editorType: 'dxTextBox',
            editorOptions: {
                placeholder: "Enter text to display"
            }
        }]
    }],
    icon: 'helloWorldItemIcon',
    title: 'Hello World Item'
};
// #endregion
// #region viewer
class HelloWorldItemViewer extends CustomItemViewer {
    renderContent(element, changeExisting) {
        element.innerText = this.getPropertyValue('customProperty');
    };
}
// #endregion
// #region createItem
class HelloWorldItem {
    constructor(dashboardControl) {
        ResourceManager.registerIcon(svgIcon);    
        this.name = "helloWorldItem";
        this.metaData = helloWorldItemMetaData;
    }

    createViewerItem(model, $element, content) {
        return new HelloWorldItemViewer(model, $element, content);
    }
}

export default HelloWorldItem;
// #endregion 
