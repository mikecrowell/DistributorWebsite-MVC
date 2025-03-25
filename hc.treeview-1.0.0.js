//-- ROOT HCE JAVASCRIPT OBJECTS --
if (typeof HCE === 'undefined') {
    var HCE = {};
}

if (typeof HCE.TreeView === 'undefined') {
    HCE.TreeView = {};
}

if (typeof HCE.TreeView === 'undefined') {
    HCE.TreeView = {};
}

//-----------------------------------------------------------------------------------------------
// FUNCTION:    HCE.TreeView.ConvertListToTree
// PURPOSE:     BUILD THE TREE FROM THE SPECIFIED LIST OF OBJECTS
//-----------------------------------------------------------------------------------------------
HCE.TreeView.ConvertListToTree = function (list, idField, parentidField, sequenceField, expandTopLevel) {

    //-- GENERATE THE NESTED TREE STRUCTURE --
    list = HCE.TreeView.GetChildNodes(list, null, idField, parentidField, sequenceField, expandTopLevel);

    //-- PASS 3 - ENABLE/DISABLE ACTIONS --
    HCE.TreeView.EnableDisableActions(list);

    return (list);
}

//------------------------------------------------------------------------------------
// FUNCTION:    HCE.TreeView.GetChildNotes
// PURPOSE:     GET THE CHILD HIERARCHY FOR THE SPECIFIED LIST 
//------------------------------------------------------------------------------------
HCE.TreeView.GetChildNodes = function (list, parentID, idField, parentIDField, sequenceField, expandTopLevel) {
    var tree = [];

    //-- FIND THE MATCHING NODES --
    if (parentID === null)
        tree = $.grep(list, function (e) { return e[parentIDField] === null });
    else
        tree = $.grep(list, function (e) { return e[parentIDField] === parentID });

    if (tree != null && tree.length > 0) {

        //-- SORT THE TREE --
        tree.sort(function (a, b) {
            return parseInt(a[sequenceField]) - parseInt(b[sequenceField]);
        });

        //-- ADD ANY CHILD ITEMS --
        for (var i = 0; i < tree.length; i++) {

            tree[i].HCTVParent = null;
            tree[i].HCTVFocus = false;
            tree[i].HCTVVisible = true;

            if (expandTopLevel === true && tree[i][parentIDField] === null)
                tree[i].HCTVStatus = 'EXPANDED';
            else
                tree[i].HCTVStatus = 'COLLAPSED';

            var children = HCE.TreeView.GetChildNodes(list, tree[i][idField], idField, parentIDField, sequenceField, expandTopLevel);
            if (children !== null && children.length > 0)
                tree[i].HCTVChildren = children;
            else
                tree[i].HCTVChildren = [];
        }
    }

    return (tree);
}

//---------------------------------------------------------------------------------------------
// FUNCTION:    HCE.TreeView.SortTreeLevels
// PURPOSE:     SORT ALL LEVELS OF THE TREE BY THE SPECIFIED SEQUENCE FIELD
//---------------------------------------------------------------------------------------------
HCE.TreeView.SortTreeLevels = function (list, level, sequenceField) {
    //-- SORT THE ARRAY --
    list.sort(function (a, b) {
        return parseInt(a[sequenceField]) - parseInt(b[sequenceField]);
    });

    //-- SORT ANY CHILD ARRAYS --
    for (var i = 0; i < list.length; i++) {
        list[i].HCTVLevel = level;
        if (typeof list[i].HCTVChildren !== 'undefined' && list[i].HCTVChildren !== null && list[i].HCTVChildren.length > 0)
            HCE.TreeView.SortTreeLevels(list[i].HCTVChildren, level + 1, sequenceField);
    }
}

//-----------------------------------------------------------------------------------------------
// FUNCTION:    HCE.TreeView.Filter
// PURPOSE:     FILTER THE TREE
//-----------------------------------------------------------------------------------------------
HCE.TreeView.FilterTree = function (list, searchField, searchText, level) {
    if ((typeof level === 'undefined') || (level === undefined) || (level === null))
        level = 0;

    //-- FILTER THE SEARCH RESULTS --
    for (var i = 0; i < list.length; i++) {
        list[i].HCTVFocus = false;
        list[i].HCTVVisible = false;

        if (searchText === '')
            list[i].HCTVVisible = true;
        else if (list[i][searchField].toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
            list[i].HCTVVisible = true;
            list[i].HCTVFocus = true;

            //-- MAKE SURE ALL PARENT RECORDS ARE EXPANDED AND VISIBLE --

        }

        if (typeof list[i].HCTVChildren !== 'undefined' && list[i].HCTVChildren !== null && list[i].HCTVChildren.length > 0)
            HCE.TreeView.FilterTree(list[i].HCTVChildren, searchField, searchText, level + 1);
    }

    if (level == 0)
        HCE.TreeView.EnsureFilteredItemsAreVisible(list);
}

//-----------------------------------------------------------------------------------------------------
// FUNCTION:    HCE.TreeView.EnsureFilteredItemsAreVisible
// PURPOSE:     MAKE SURE ALL THE FILTERED ITEMS ARE VISIBLE UP TO THE TOP LEVEL PARENT ITEM
//-----------------------------------------------------------------------------------------------------
HCE.TreeView.EnsureFilteredItemsAreVisible = function (list) {
    var curParent;

    //-- FILTER THE SEARCH RESULTS --
    for (var i = 0; i < list.length; i++) {
        if (HCE.TreeView.HasVisibleChildren(list[i])) {
            list[i].HCTVVisible = true;
            list[i].HCTVStatus = 'EXPANDED';
        }
        else
            list[i].HCTVStatus = 'COLLAPSED';

        if (typeof list[i].HCTVChildren !== 'undefined' && list[i].HCTVChildren !== null && list[i].HCTVChildren.length > 0) {
            HCE.TreeView.EnsureFilteredItemsAreVisible(list[i].HCTVChildren);
        }
    }
}

//------------------------------------------------------------------------------------------------------
// FUNCTION:    HCE.TreeView.HasVisibleChildren
// PURPOSE:     CHECK TO SEE IF THE TREE HAS ANY VISIBLE CHILDREN
//------------------------------------------------------------------------------------------------------
HCE.TreeView.HasVisibleChildren = function (entity) {
    if (entity.HCTVVisible === true)
        return (true);

    if (typeof entity.HCTVChildren !== 'undefined' && entity.HCTVChildren !== null && entity.HCTVChildren.length > 0) {
        for (var i = 0; i < entity.HCTVChildren.length; i++) {
            if (entity.HCTVChildren[i].HCTVVisible === true)
                return (true);
            else if (HCE.TreeView.HasVisibleChildren(entity.HCTVChildren[i]))
                return (true);
        }
    }

    return (false);
}

//-------------------------------------------------------------
//-- FUNCTION:  HCE.TreeView.EnsureVisible
//-- PURPOSE:   MAKE SURE THE ITEM IS VISIBLE UP TO THE PARENT
//-------------------------------------------------------------
HCE.TreeView.EnsureVisible = function (list, id, idField) {
    for (var i = 0; i < list.length; i++) {
        if (list[i][idField] == id) {
            list[i].HCTVVisible = true;
            list[i].HCTVStatus = 'EXPANDED';
            break;
        }
        else if (typeof list[i].HCTVChildren !== 'undefined' && list[i].HCTVChildren !== null && list[i].HCTVChildren.length > 0) {
            if (HCE.TreeView.IsIDInTree(list[i].HCTVChildren, id, idField)) {
                list[i].HCTVVisible = true;
                list[i].HCTVStatus = 'EXPANDED';

                HCE.TreeView.EnsureVisible(list[i].HCTVChildren, id, idField);
            }

        }
    }
}

//----------------------------------------------------------------
//-- FUNCTION:  HCE.TreeView.IsIDInTree
//-- PURPOSE:   CHECK TO SEE IF THE SPECIFIED ID IS IN THE TREE
//----------------------------------------------------------------
HCE.TreeView.IsIDInTree = function (list, id, idField) {
    for (var i = 0; i < list.length; i++) {
        if (list[i][idField] === id)
            return (true);
        else if (HCE.TreeView.IsIDInTree(list[i].HCTVChildren, id, idField))
            return (true);
    }

    return (false);
}

//-------------------------------------------------------------
//-- FUNCTION:  HCE.TreeView.MoveItem
//-- PURPOSE:   MOVE THE SPECIFIED ITEM UP OR DOWN
//-------------------------------------------------------------
HCE.TreeView.MoveItem = function (list, id, idField, direction) {
    var movedItem = false;

    for (var i = 0; i < list.length; i++) {
        if (list[i][idField] === id) {
            //-- MOVE THE ITEM --
            movedItem = true;

            if ((direction === 'UP') && (i > 0)) {
                var currentItem = list[i];
                list[i] = list[i - 1];
                list[i - 1] = currentItem;
            }
            else if ((direction === 'DOWN') && (i < list.length - 1)) {
                var currentItem = list[i];
                list[i] = list[i + 1];
                list[i + 1] = currentItem;
            }
        }
        else
            movedItem = HCE.TreeView.MoveItem(list[i].HCTVChildren, id, idField, direction);

        if (movedItem)
            break;
    }

    return (false);
}

//-------------------------------------------------------------
//-- FUNCTION:  HCE.TreeView.RemoveItem
//-- PURPOSE:   MOVE THE SPECIFIED ITEM UP OR DOWN
//-------------------------------------------------------------
HCE.TreeView.RemoveItem = function (list, id, idField) {
    var deletedItem = false;

    for (var i = 0; i < list.length; i++) {
        if (list[i][idField] === id) {
            //-- DELETE THE ITEM --
            deletedItem = true;
            list.splice(i, 1);
        }
        else
            deletedItem = HCE.TreeView.RemoveItem(list[i].HCTVChildren, id, idField);

        if (deletedItem)
            break;
    }

    return (false);
}

//-------------------------------------------------------------
//-- FUNCTION:  HCE.TreeView.UpdateItem
//-- PURPOSE:   UPDATE THE SPECIFIED ITEM
//-------------------------------------------------------------
HCE.TreeView.UpdateItem = function (list, id, idField, parentIDField, entity, topLevelList) {
    var updatedItem = false;

    //-- INITIALIZE THE TOP LEVEL LIST IN CASE WE NEED TO DELETE AND READD --
    if (typeof topLevelList === 'undefined' || topLevelList === null)
        topLevelList = list;

    for (var i = 0; i < list.length; i++) {
        if (list[i][idField] === id) {
            if (list[i][parentIDField] !== entity[parentIDField]) {

                //-- DELETE AND READD --
                list.splice(i, 1);
                HCE.TreeView.AddItem(topLevelList, idField, parentIDField, entity);

            }
            else {
                //-- UPDATE THE PROPERTIES FOR THE EXISTING ITEM WITH THE UPDATED ENTITY --
                updatedItem = true;

                for (var property in entity) {
                    if (entity.hasOwnProperty(property)) {
                        list[i][property] = entity[property];
                    }
                }
            }
        }
        else
            updatedItem = HCE.TreeView.UpdateItem(list[i].HCTVChildren, id, idField, parentIDField, entity, topLevelList);

        if (updatedItem)
            break;
    }

    return (false);
}

//-------------------------------------------------------------
//-- FUNCTION:  HCE.TreeView.AddItem
//-- PURPOSE:   ADD A NEW ITEM
//-------------------------------------------------------------
HCE.TreeView.AddItem = function (list, idField, parentIDField, entity) {
    var addedItem = false;

    entity.HCTVFocus = true;
    entity.HCTVStatus = 'EXPANDED';
    entity.HCTVVisible = true;
    entity.HCTVChildren = [];

    if (typeof parentIDField === 'undefined' || parentIDField === null || entity[parentIDField] == null) {
        //-- ADD THE ROOT ITEM --
        addedItem = true;
        entity.HCTVParent = null;
        list.push(entity);
        return;
    }

    for (var i = 0; i < list.length; i++) {
        if (list[i][idField] === entity[parentIDField]) {

            //-- UPDATE THE PROPERTIES FOR THE EXISTING ITEM WITH THE UPDATED ENTITY --
            entity.HCTVParent = list[i];
            list[i].HCTVChildren.push(entity);
            return (true);
        }
        else
            addedItem = HCE.TreeView.AddItem(list[i].HCTVChildren, idField, parentIDField, entity);
    }

    return (false);
}

//---------------------------------------------------------------------------------------------
// FUNCTION:    HCE.TreeView.EnableDisableActions
// PURPOSE:     ENABLE/DISABLE THE NODE ACTIONS
//---------------------------------------------------------------------------------------------
HCE.TreeView.EnableDisableActions = function (list, idToHighlight, idFieldName, idFieldLevel, level) {

    if ((typeof level === 'undefined') || (level === undefined) || (level === null))
        level = 0;

    if ((typeof idFieldLevel === 'undefined') || (idFieldLevel === undefined) || (idFieldLevel === null))
        idFieldLevel = -1;

    //-- SORT ANY CHILD ARRAYS --
    for (var i = 0; i < list.length; i++) {
        list[i].HCTVEnableUp = (i > 0);
        list[i].HCTVEnableDown = (i < (list.length - 1));
        list[i].HCTVFocus = false;

        if (typeof idToHighlight !== 'undefined' && idToHighlight !== undefined && idToHighlight !== null) {
            if ((idFieldLevel == -1) || (idFieldLevel == level)) {
                if (list[i][idFieldName] === idToHighlight)
                    list[i].HCTVFocus = true;
            }
        }

        if (typeof list[i].HCTVChildren !== 'undefined' && list[i].HCTVChildren !== null && list[i].HCTVChildren.length > 0) {
            list[i].HCTVEnableDelete = false;
            HCE.TreeView.EnableDisableActions(list[i].HCTVChildren, idToHighlight, idFieldName, idFieldLevel, level + 1);
        }
        else
            list[i].HCTVEnableDelete = true;
    }
}

//---------------------------------------------------------------------------------------------
// FUNCTION:    HCE.TreeView.ExpandCollapseAll
// PURPOSE:     ENABLE/DISABLE THE NODE ACTIONS
//---------------------------------------------------------------------------------------------
HCE.TreeView.ExpandCollapseAll = function (list, action) {
    //-- SORT ANY CHILD ARRAYS --
    for (var i = 0; i < list.length; i++) {
        if (action == 'EXPAND')
            list[i].HCTVStatus = 'EXPANDED';
        else
            list[i].HCTVStatus = 'COLLAPSED';

        if (typeof list[i].HCTVChildren !== 'undefined' && list[i].HCTVChildren !== null && list[i].HCTVChildren.length > 0)
            HCE.TreeView.ExpandCollapseAll(list[i].HCTVChildren, action);
    }
}