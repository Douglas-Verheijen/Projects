<div class="artistComponents">
    <?php foreach ($model->Art as $key=>$art) : ?>
        <a href="<?php echo $art->Thumbnail; ?>" data-featherlight="#image_<?php echo $key; ?>">
            <div class="artistGalleryComponent" style="height: <?php echo $model->MaxHeight; ?>px;">
                <img class="artistGalleryComponentImage" src="<?php echo $art->Thumbnail; ?>">
                <div class="artistGalleryComponentHighlight">
                    <?php echo $art->Name; ?>
                </div>
                <div id="image_<?php echo $key; ?>" hidden>
                    <div class="product-info-wrapper">
                            <div class="product-info">
                                <div class="product-info-heading"><?php echo $art->Name; ?></div>
                                <div class="product-info-content">
                                    <img class="artistGalleryComponentImage" src="<?php echo $art->Thumbnail; ?>"> <br />
                                    <?php echo $art->Name; ?><br />
                                    <?php echo $art->Date; ?><br />
                                    <?php echo $art->Price.".00"; ?><br />
                                    <?php echo $art->Medium; ?><br />
                                    <?php echo $art->Size; ?><br />
                                    <?php echo $art->Status; ?>
                                </div>
                            </div>
                    </div>
                </div>
            </div>
        </a>
    <?php endforeach ?>
</div>